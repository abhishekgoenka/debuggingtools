using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Auditor.Utilities
{
    public class Diagnostics
    {
        private const int MaxInnerExceptionDepth = 16;

        /// <summary>
        /// Gets unique list of assemblies involved in exception's stack trace along with their product versions
        /// This helps to identify what particular version on the client is involved with the exception.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static IList<string> GetStackTraceAssemblies(Exception ex)
        {
            Dictionary<string, string> assemblyDict = new Dictionary<string, string>();
            List<string> assemblies = new List<string>();
            StackTrace trace = new StackTrace(ex, true);
            StackFrame[] frames = trace.GetFrames();
            if (frames == null)
                return null;

            foreach (var stackFrame in frames)
            {
                MethodBase method = stackFrame.GetMethod();
                string moduleName = method.Module.Name;
                if (!assemblyDict.ContainsKey(moduleName))
                {
                    assemblyDict[moduleName] = FileVersionInfo.GetVersionInfo(method.Module.Assembly.Location).ProductVersion;
                }
            }

            assemblies.AddRange(assemblyDict.Keys.Select(key => string.Format("{0}: {1}", key, assemblyDict[key])));

            return assemblies;
        }

        /// <summary>
        /// Gets unique list of assemblies involved in exception's stack trace along with their product versions
        /// This helps to identify what particular version on the client is involved with the exception.
        /// Optionally includes results for inner exceptions
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="includeInnerExceptions"></param>
        /// <returns></returns>
        public static IList<string> GetStackTraceAssemblies(Exception exception, bool includeInnerExceptions)
        {
            if (!includeInnerExceptions)
                return GetStackTraceAssemblies(exception);


            IList<string> result = GetStackTraceAssemblies(exception);

            if (result == null) return new List<string>();

            int exceptionDepth = 0;
            Exception innerException = exception.InnerException;
            while (exceptionDepth < MaxInnerExceptionDepth && innerException != null)
            {
                exceptionDepth++;
                IList<string> assemblies = GetStackTraceAssemblies(innerException);

                if (assemblies.Count > 0)
                {
                    foreach (string assembly in assemblies)
                        if (!result.Contains(assembly))
                            result.Add(assembly);
                }
                else // could not get assemblies - no need to continue.
                    break;

                innerException = innerException.InnerException;
            }

            return result;
        }

        /// <summary>
        /// Constructs stack trace frame by frame
        /// Records IL & Metadata offsets, so line numbers can be obtained from the symbol server.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string GetStackTrace(Exception ex)
        {
            try
            {
                StackTrace trace = new StackTrace(ex, true);
                StackFrame[] frames = trace.GetFrames();
                if (frames == null)
                    return string.Empty;

                StringBuilder sb = new StringBuilder();
                foreach (var stackFrame in frames)
                {
                    MethodBase method = stackFrame.GetMethod();
                    string moduleName = method.Module.Name;
                    int methodToken = method.MetadataToken;
                    int ilOffset = stackFrame.GetILOffset();
                    string methodString = method.ToString();
                    if (method.DeclaringType != null)
                    {
                        string frameString = String.Format("  at {0}.{1} [{2},{3},{4}]",
                                                           method.DeclaringType.FullName,
                                                           methodString.Substring(methodString.IndexOf(' ') + 1),
                                                           moduleName, ilOffset, methodToken);

                        sb.AppendLine(frameString);
                    }
                }
                return sb.ToString();
            }
            catch (Exception) // get default stack trace if failed for any reason.
            {
                return ex.StackTrace.Trim();
            }


        }

        /// <summary>
        /// Constructs stack trace frame by frame
        /// Records IL & Metadata offsets, so line numbers can be obtained from the symbol server.
        /// Optionally includes results for inner exceptions.
        /// </summary>
        /// <param name="exception">current exception</param>
        /// <param name="includeInnerExceptions">TRUE if want to include innerException</param>
        /// <returns>Exception array</returns>
        public static IList<string> GetStackTrace(Exception exception, bool includeInnerExceptions)
        {

            IList<string> result = new List<string>();
            result.Add(GetStackTrace(exception));

            if (!includeInnerExceptions)
            {
                return result;
            }

            // inner exceptions
            int exceptionDepth = 0;
            Exception innerException = exception.InnerException;
            while (exceptionDepth < MaxInnerExceptionDepth && innerException != null)
            {
                exceptionDepth++;
                string trace = GetStackTrace(innerException);
                if (!string.IsNullOrEmpty(trace))
                {
                    result.Add(trace);
                }
                else // could not get stack - no need to continue.
                    break;

                innerException = innerException.InnerException;
            }
            return result;

        }

        private static string GetExceptionMessage(Exception ex)
        {
            return ex.Message;
        }

        public static String GetExceptionMessage(Exception exception, bool includeInnerExceptions)
        {

            String result = String.Empty;
            result += GetExceptionMessage(exception);

            if (!includeInnerExceptions)
                return result;

            int exceptionDepth = 0;
            Exception innerException = exception.InnerException;
            while (exceptionDepth < MaxInnerExceptionDepth && innerException != null)
            {
                exceptionDepth++;
                string message = GetExceptionMessage(innerException);
                if (!string.IsNullOrEmpty(message))
                {
                    result += message;
                }
                else // could not get message - no need to continue.
                    break;

                innerException = innerException.InnerException;
            }
            return result;

        } 
    }
}