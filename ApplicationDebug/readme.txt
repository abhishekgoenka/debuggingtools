Ref: http://msdn.microsoft.com/en-us/library/aa384088.aspx
--Specifies that access is granted to a private key for Networkservice account
winhttpcertcfg -g -c LOCAL_MACHINE\My -s EEHRIdPSigning -a NETWORKSERVICE

--List Certificates
winhttpcertcfg -l -c LOCAL_MACHINE\My -s EEHRIdPSigning

certmgr.msc
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

