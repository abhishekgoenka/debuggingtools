// Sample to demonstrate creating a stack trace with source location information while controlling
// how PDB files are located.
// Written by Rick Byers - http://blogs.msdn.com/rmbyers
// 6/21/2007 - Initial version 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Reflection;

// Use MDbg's managed wrappers over the corysm.idl (diasymreader.dll) COM APIs
// Must reference MDbgCore.dll from the .NET SDK or corapi.dll from the MDbg sample: 
// http://www.microsoft.com/downloads/details.aspx?familyid=38449a42-6b7a-4e28-80ce-c55645ab1310&displaylang=en
using Microsoft.Samples.Debugging.CorSymbolStore;

namespace StackProvider
{
    /// <summary>
    /// A class for producing stack traces with file and line number information using custom PDB
    /// lookup logic.
    /// </summary>
    /// <remarks>
    /// The CLR's StackTrace class will only load PDBs that are next to their corresponding module (or
    /// in a few other standard locations like the _NT_SYMBOL_PATH environment variable and system directory).
    /// PDBs are considered a development-time-only scenario (not intended for use in production), and so usually 
    /// are directly next to the image.  However, this is sometimes too restrictive for some development/testing 
    /// scenarios.  Use this class to get stack traces with full source info when you want to find PDBs elsewhere, 
    /// such as in specific paths or on a symbol server.
    /// 
    /// An alternate (often superior) approach that could be taken using the same basic code would be to
    /// save the stack traces in a computer readable form (XML perhaps) with module names, method tokens
    /// and IL offsets.  Then build a tool that takes this as input and after-the-fact loads PDBs to
    /// create a full stack trace with source information.  The main benefit of this is that it avoids
    /// having to make your PDBs available to all the test machines running your code.
    /// 
    /// For error-reporting and diagnosis scenarios in production, Microsoft suggests the use of Windows
    /// Error Reporting (https://winqual.microsoft.com/).  
    ///  
    /// Note that some of this code is adapted from http://blogs.msdn.com/jmstall/pages/sample-pdb2xml.aspx
    /// </remarks>
    public class StackTraceSymbolProvider
    {
        /// <summary>
        /// Create a new instance with a specified policy for finding symbols
        /// </summary>
        /// <param name="searchPath">A semi-colon separated list of additional paths to check</param>
        /// <param name="searchPolicy">A set of flags saying how symbols can be located:
        ///     AllowRegistryAccess - allow lookup in paths specified in the registry (not sure where exactly)
        ///     AllowSymbolServerAccess - allow paths starting with "srv*" to load PDBs from a symbol server
        ///     AllowOriginalPathAccess - will look in the original directory the PDB was built into
        ///     AllowReferencePathAccess - will look in the directory next to the exe/dll
        /// </param>
        public StackTraceSymbolProvider(string searchPath, SymSearchPolicies searchPolicy)
        {
            m_searchPath = searchPath;
            m_searchPolicy = searchPolicy;

            // Create a COM Metadata dispenser to use for all modules
            Guid dispenserClassID = new Guid(0xe5cb7a31, 0x7512, 0x11d2, 0x89, 0xce, 0x00, 0x80, 0xc7, 0x92, 0xe5, 0xd8); // CLSID_CorMetaDataDispenser
            Guid dispenserIID = new Guid(0x809c652e, 0x7396, 0x11d2, 0x97, 0x71, 0x00, 0xa0, 0xc9, 0xb4, 0xd5, 0x0c); // IID_IMetaDataDispenser
            object objDispenser;
            CoCreateInstance(ref dispenserClassID, null, 1, ref dispenserIID, out objDispenser);
            m_metadataDispenser = (IMetaDataDispenser)objDispenser;

            // Create a binder from MDbg's wrappers over ISymUnmanagedBinder2
            m_symBinder = new SymbolBinder();
        }

        /// <summary>
        /// Create a symbol reader object corresponding to the specified module (DLL/EXE)
        /// </summary>
        /// <param name="modulePath">Full path to the module of interest</param>
        /// <returns>A symbol reader object, or null if no matching PDB symbols can located</returns>
        private ISymbolReader CreateSymbolReaderForFile(string modulePath)
        {
            // First we need to get a metadata importer for the module to provide to the symbol reader
            // This is basically the same as MDbg's SymbolAccess.GetReaderForFile method, except that it
            // unfortunately does not have an overload that allows us to provide the searchPolicies
            Guid importerIID = new Guid(0x7dac8207, 0xd3ae, 0x4c75, 0x9b, 0x67, 0x92, 0x80, 0x1a, 0x49, 0x7d, 0x44); // IID_IMetaDataImport

            // Open an Importer on the given filename. We'll end up passing this importer straight
            // through to the Binder.
            object objImporter;
            m_metadataDispenser.OpenScope(modulePath, 0, ref importerIID, out objImporter);
            
            // Call ISymUnmanagedBinder2.GetReaderForFile2 to load the PDB file (if any)
            // Note that ultimately how this PDB file is located is determined by
            // IDiaDataSource::loadDataForExe.  See the DIA SDK documentation for details.
            ISymbolReader reader = m_symBinder.GetReaderForFile(objImporter, modulePath, m_searchPath, m_searchPolicy);
            return reader;
        }

        /// <summary>
        /// Get or create a symbol reader for the specified module (caching the result)
        /// </summary>
        /// <param name="modulePath">Full path to the module of interest</param>
        /// <returns>A symbol reader for the specified module or null if none could be found</returns>
        private ISymbolReader GetSymbolReaderForFile(string modulePath)
        {
            ISymbolReader reader;
            if (!m_symReaders.TryGetValue(modulePath, out reader))
            {
                reader = CreateSymbolReaderForFile(modulePath);
                m_symReaders.Add(modulePath, reader);
            }
            return reader;
        }

        /// <summary>
        /// Get a texual representing of the supplied stack trace including source file names
        /// and line numbers, using the PDB lookup options supplied at construction.
        /// </summary>
        /// <param name="stackTrace">The stack trace to convert to text</param>
        /// <returns>A string in a format similar to StackTrace.ToString but whith file names and
        /// line numbers even when they're not available to the built-in StackTrace class.</returns>
        public string StackTraceToStringWithSourceInfo(StackTrace stackTrace)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach(StackFrame stackFrame in stackTrace.GetFrames())
            {
                MethodBase method = stackFrame.GetMethod();

                // Format the stack trace line similarily to how the built-in StackTrace class does.
                // Some differences (simplifications here): generics, nested types, argument names
                string methodString = method.ToString();   // this is "RetType FuncName(args)
                string sig = String.Format("  at {0}.{1}",
                    method.DeclaringType.FullName,
                    methodString.Substring(methodString.IndexOf(' ')+1));

                // Append source location information if we can find PDBs
                string sourceLoc = GetSourceLoc(method, stackFrame.GetILOffset());
                if (sourceLoc != null)
                    sig += " in " + sourceLoc;
                
                sb.AppendLine(sig);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get a string representing the source location for the given IL offset and method
        /// </summary>
        /// <param name="method">The method of interest</param>
        /// <param name="ilOffset">The offset into the IL</param>
        /// <returns>A string of the format [filepath]:[line] (eg. "C:\temp\foo.cs:123"), or null
        /// if a matching PDB couldn't be found</returns>
        private string GetSourceLoc(MethodBase method, int ilOffset)
        {
            // Get the symbol reader corresponding to the module of the supplied method
            string modulePath = method.Module.FullyQualifiedName;
            ISymbolReader symReader = GetSymbolReaderForFile(modulePath);
            if (symReader == null)
                return null;    // no matching PDB found

            ISymbolMethod symMethod = symReader.GetMethod(new SymbolToken(method.MetadataToken));

            // Get all the sequence points for the method
            ISymbolDocument [] docs = new ISymbolDocument[symMethod.SequencePointCount];
            int [] lineNumbers = new int[symMethod.SequencePointCount];
            int [] ilOffsets = new int[symMethod.SequencePointCount];
            symMethod.GetSequencePoints(ilOffsets, docs, lineNumbers, null, null, null);

            // Find the closest sequence point to the requested offset
            // Sequence points are returned sorted by offset so we're looking for the last one with
            // an offset less than or equal to the requested offset. 
            // Note that this won't necessarily match the real source location exactly if 
            // the code was jit-compiled with optimizations.
            int i;
            for (i = 0; i < symMethod.SequencePointCount; i++)
            {
                if (ilOffsets[i] > ilOffset)
                    break;
            }
            // Found the first mismatch, back up if it wasn't the first
            if (i > 0)
                i--;

            // Now return the source file and line number for this sequence point
            return String.Format("{0}:{1}", docs[i].URL, lineNumbers[i]);
        }

        public string GetSourceLoc(string methodModulePath, int methodMetadataToken, int ilOffset)
        {
            // Get the symbol reader corresponding to the module of the supplied method
            ISymbolReader symReader = null; 
            try
            {
                symReader = GetSymbolReaderForFile(methodModulePath);
            }
            catch {}

            if (symReader == null)
                return string.Empty; ;    // no matching PDB found

            ISymbolMethod symMethod = symReader.GetMethod(new SymbolToken(methodMetadataToken));

            // Get all the sequence points for the method
            ISymbolDocument[] docs = new ISymbolDocument[symMethod.SequencePointCount];
            int[] lineNumbers = new int[symMethod.SequencePointCount];
            int[] ilOffsets = new int[symMethod.SequencePointCount];
            symMethod.GetSequencePoints(ilOffsets, docs, lineNumbers, null, null, null);

            // Find the closest sequence point to the requested offset
            // Sequence points are returned sorted by offset so we're looking for the last one with
            // an offset less than or equal to the requested offset. 
            // Note that this won't necessarily match the real source location exactly if 
            // the code was jit-compiled with optimizations.
            int i;
            for (i = 0; i < symMethod.SequencePointCount; i++)
            {
                if (ilOffsets[i] > ilOffset)
                    break;
            }
            // Found the first mismatch, back up if it wasn't the first
            if (i > 0)
                i--;

            // Now return the source file and line number for this sequence point
            return String.Format("{0}:{1}", docs[i].URL, lineNumbers[i]);
        }


        // We could easily add other APIs similar to those available on StackTrace (and StackFrame)

        private IMetaDataDispenser m_metadataDispenser;
        private SymbolBinder m_symBinder;
        private string m_searchPath;
        private SymSearchPolicies m_searchPolicy;

        // Map from module path to symbol reader
        private Dictionary<string,ISymbolReader> m_symReaders = new Dictionary<string,ISymbolReader>();
        
        #region Metadata Imports

        // Bare bones COM-interop definition of the IMetaDataDispenser API
        [Guid("809c652e-7396-11d2-9771-00a0c9b4d50c"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComVisible(true)]
        private interface IMetaDataDispenser
        {
            // We need to be able to call OpenScope, which is the 2nd vtable slot.
            // Thus we need this one placeholder here to occupy the first slot..
            void DefineScope_Placeholder();

            //STDMETHOD(OpenScope)(                   // Return code.
            //LPCWSTR     szScope,                // [in] The scope to open.
            //  DWORD       dwOpenFlags,            // [in] Open mode flags.
            //  REFIID      riid,                   // [in] The interface desired.
            //  IUnknown    **ppIUnk) PURE;         // [out] Return interface on success.
            void OpenScope([In, MarshalAs(UnmanagedType.LPWStr)] String szScope, [In] Int32 dwOpenFlags, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.IUnknown)] out Object punk);

            // Don't need any other methods.
        }

        // Since we're just blindly passing this interface through managed code to the Symbinder, we don't care about actually
        // importing the specific methods.
        // This needs to be public so that we can call Marshal.GetComInterfaceForObject() on it to get the
        // underlying metadata pointer.
        [Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComVisible(true)]
        public interface IMetadataImport
        {
            // Just need a single placeholder method so that it doesn't complain about an empty interface.
            void Placeholder();
        }
        #endregion

        [DllImport("ole32.dll")]
        private static extern int CoCreateInstance([In] ref Guid rclsid,
                                                   [In, MarshalAs(UnmanagedType.IUnknown)] Object pUnkOuter,
                                                   [In] uint dwClsContext,
                                                   [In] ref Guid riid,
                                                   [Out, MarshalAs(UnmanagedType.Interface)] out Object ppv);

    }
}
