USE Works

--Get user
SELECT * FROM dbo.IDX_User WHERE AUsername LIKE 'abhi1_mar10'

--Check if Provider has EPCS Permission
SELECT dbo.fnProviderHasEPCSPermission(1015)

--Check for precondition
SELECT dbo.fnEPCSPreconditionValidationFailedErrorMsg(1015)

--Get list of Approvers
SELECT * FROM dbo.ShieldRegistration WHERE IsEPCSApprover = 1

--Get Shield User info
SELECT * FROM dbo.ShieldRegistration WHERE UserID = 1015

--Shield audit logs
SELECT dbo.ShieldRegistration_Audit.ID, UserID, ModifiedDTTM, ModifiedByID, ColumnName, OldValue, NewValue, SDE1.EntryName, SDE2.EntryName FROM dbo.ShieldRegistration_Audit WITH (NOLOCK) JOIN dbo.ShieldUserStatus_DE SDE1 
 ON SDE1.ID = ShieldRegistration_Audit.OldValue 
 JOIN dbo.ShieldUserStatus_DE SDE2
 ON SDE2.ID = dbo.ShieldRegistration_Audit.NewValue
 WHERE UserID = 1332
 ORDER BY ModifiedDTTM ASC
 
--Restart GRANT process again
--Get ID of ENROLL
select ID from dbo.ShieldUserStatus_DE with (nolock) Where  EntryCode = 'ENROLL'

--Update provider status to ENROLL
UPDATE dbo.ShieldRegistration SET ShieldUserStatusID = <@ENROLL ID> WHERE UserID = <UserId>
---------------------------------------------------------------------------------------------


