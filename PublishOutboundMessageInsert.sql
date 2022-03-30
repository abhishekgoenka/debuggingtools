
--Insert OUTBound Message
BEGIN TRANSACTION
DECLARE @message VARCHAR(MAX)
DECLARE @jobID INT

INSERT INTO dbo.EEHR_Message_Outbound
        ( ReplyToMessageID ,
          MessageType ,
          MessageStatus ,
          CreatedDTTM ,
          LastUpdatedDTTM ,
          PublishedDTTM ,
          DocumentID ,
          MessageContent ,
          PatientID ,
          ProviderID ,
          UserID ,
          Fault ,
          MessageRecipient ,
          MessageID ,
          MessageEvent
        )
VALUES  ( NULL , -- ReplyToMessageID - uniqueid
          'Allscripts.Prescription.Eventing.RxPrintedEvent, Allscripts.Prescription, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' , -- MessageType - varchar(255)
          'Published' , -- MessageStatus - varchar(50)
          '2013-12-29 02:33:49' , -- CreatedDTTM - datetime
          '2013-12-29 02:33:49' , -- LastUpdatedDTTM - datetime
          '2013-12-29 02:33:49' , -- PublishedDTTM - datetime
          0 , -- DocumentID - uniqueid
          '<RxPrintedEvent xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ID>9999140001156981</ID>
  <TimeStamp>12/18/2013 5:16:18 AM</TimeStamp>
  <Destination />
  <Noun>Rx</Noun>
  <Verb>Printed</Verb>
  <KeyedValues>
    <KeyedValue>
      <Key>ReplyToMessageID</Key>
      <Value>156981</Value>
    </KeyedValue>
    <KeyedValue>
      <Key>DestinationID</Key>
      <Value />
    </KeyedValue>
    <KeyedValue>
      <Key>PatientID</Key>
      <Value>93259</Value>
    </KeyedValue>
    <KeyedValue>
      <Key>AccountID</Key>
      <Value>00000000000009999140</Value>
    </KeyedValue>
    <KeyedValue>
      <Key>UserID</Key>
      <Value>92025</Value>
    </KeyedValue>
    <KeyedValue>
      <Key>ProductID</Key>
      <Value>TW</Value>
    </KeyedValue>
    <KeyedValue>
      <Key>ProductVersion</Key>
      <Value>11.4.1.612.112</Value>
    </KeyedValue>
  </KeyedValues>
  <PrescriptionMessage>
    <Switch>
      <TestIndicator />
      <RecipientID />
      <RecipientIDQualfier />
      <RecipientIdLevelTwo />
      <RecipientIdLevelThree />
      <SenderID>9999140001          </SenderID>
      <SenderIDQualifier />
      <SenderPassword />
      <SenderIdLevelThree />
      <AppID>TW</AppID>
      <AppVersion>11.4.1.612.112</AppVersion>
      <MedHistoryRequest />
      <ReturnReceipt />
      <PartnerPortal />
      <WellPointMmhp />
      <ComputeMAI />
      <ScriptSwId />
      <MessageType>PRTRX </MessageType>
      <TransactionControlNumber>9999140001156981</TransactionControlNumber>
      <MessageReferenceNumber />
      <InitiatedTransactionControlReferecenID />
      <ProviderID>00000000000009999140_92025</ProviderID>
      <IsItEPCS />
      <SentDateTime />
    </Switch>
    <Prescription>
      <PrescriberOrderNo />
      <RxID>P</RxID>
      <RxNumber />
      <NDCNumber>65862007175</NDCNumber>
      <DDI>60104</DDI>
      <DrugDescription>Amoxicillin 400 MG/5ML Oral Suspension Reconstituted</DrugDescription>
      <Quantity>12</Quantity>
      <QuantityQualifier>ML</QuantityQualifier>
      <RxDate>2013-12-18T00:00:00</RxDate>
      <Created>2013-12-18T00:00:00</Created>
      <IsListed>N</IsListed>
      <DrugDrugChecked>false</DrugDrugChecked>
      <DuplicateTherapyChecked>false</DuplicateTherapyChecked>
      <PARChecked>false</PARChecked>
      <DosageFormCode>SUS</DosageFormCode>
      <Strength>400</Strength>
      <StrengthUOM>400</StrengthUOM>
      <EffectiveDate />
      <SigID />
      <SIGText>TAKE 12 ML 5 times daily                                                                                                                    </SIGText>
      <RefillQuantity>0</RefillQuantity>
      <RefillQuantityQual>0</RefillQuantityQual>
      <DaysSupply>1</DaysSupply>
      <DAW>false</DAW>
      <DAWDetail>N</DAWDetail>
      <RxNotes />
      <CreatedFirstName />
      <CreatedLastName />
      <CreatedNameSuffix />
      <CreatedNamePrefix />
      <ControlledSubstanceCode />
      <OriginalDDI>0</OriginalDDI>
      <OriginalNDCNumber />
      <OriginalAHSFormuStatus>0</OriginalAHSFormuStatus>
      <OriginalSourceFormuStatus>0</OriginalSourceFormuStatus>
      <OriginalIsListed>N</OriginalIsListed>
      <ICD10Code>Z00.129</ICD10Code>
      <RxNormCUI>308189</RxNormCUI>
      <RxNormQual>SCD</RxNormQual>
      <RxKey>00000000000009999140_553175900001</RxKey>
      <FormuAltsShown>N</FormuAltsShown>
      <Compound>N</Compound>
      <Supply>N</Supply>
    </Prescription>
    <DispensedPrescription />
    <RequestedPrescription />
    <Patient>
      <Ssn>345-34-5123</Ssn>
      <MedicareNumber />
      <PbmID />
      <PbmIDExtension />
      <FirstName>Zuri</FirstName>
      <MiddleName />
      <LastName>Allscripts</LastName>
      <NamePrefix />
      <NameSuffix />
      <Dob>1999-07-09</Dob>
      <GenderCode>F  </GenderCode>
      <Address1 />
      <Address2 />
      <City />
      <State />
      <ZipCode>60005     </ZipCode>
      <PersonCode />
      <Consent />
      <RelationshipCD />
      <DataChanged />
      <MAI_PAT_DESC />
      <MAI_PAT_IND />
      <MAI_PAT_VER />
      <FileID />
      <Contact>
        <Phone />
        <Fax />
        <Email />
        <Cell />
        <Beeper />
        <HomePhone />
        <NightPhone />
      </Contact>
    </Patient>
    <Provider>
      <ProviderID>00000000000009999140_92025</ProviderID>
      <ProviderQual />
      <DeaNumber>AA5836727</DeaNumber>
      <NpiNumber>8524275020          </NpiNumber>
      <StateLicenseNumb>4.52035E+12</StateLicenseNumb>
      <MutuallyDefined />
      <AccountID>00000000000009999140</AccountID>
      <PracticeName>Primary Care Partners</PracticeName>
      <SiteProviderID />
      <FirstName>Provider</FirstName>
      <MiddleName>TW</MiddleName>
      <LastName>Allscripts</LastName>
      <Suffix />
      <Prefix />
      <Title />
      <Address1>3150 N 12th Street.2nd Main 4th C C</Address1>
      <Address2 />
      <City>Grand Hawaii</City>
      <State>IL</State>
      <ZipCode>81506</ZipCode>
      <Contact>
        <Phone>9752450110</Phone>
        <Fax />
        <Email>allscripts@allscripts.com</Email>
      </Contact>
      <AMASpecialtyCd />
    </Provider>
    <Supervisor />
    <Pharmacy />
    <Status />
    <Response />
    <DURConfig>
      <DoseMaxConsecDuration>Y</DoseMaxConsecDuration>
      <DoseMaxIndvDose>Y</DoseMaxIndvDose>
      <DoseMinDuration>N</DoseMinDuration>
      <DoseMaxDuration>Y</DoseMaxDuration>
      <DoseMinDose>N</DoseMinDose>
      <DoseMaxDose>Y</DoseMaxDose>
      <DUPScreen />
      <DUPWarnings>0    </DUPWarnings>
      <DrugDrugOnset>1    </DrugDrugOnset>
      <DrugDrugMinSeverity>1    </DrugDrugMinSeverity>
      <DrugDrugMinDoc>2    </DrugDrugMinDoc>
      <DiseaseContraindication>0    </DiseaseContraindication>
    </DURConfig>
    <DURAlert />
    <Encounter />
    <CardHolder>
      <CardHolderID />
      <CardHolderOrgName />
      <PbmID />
      <PbmName />
      <AlternateID />
      <LastName />
      <FirstName />
      <MiddleName />
      <CardHolderName />
      <Address1 />
      <City />
      <State />
      <ZipCode>60005     </ZipCode>
      <Dob>1999-07-09</Dob>
      <GenderCode>F  </GenderCode>
      <CrdHldrBinNo />
      <CrdHldrProcCntrlNo />
      <GroupID />
      <MutuallyDefined />
      <InfoSourcePayerID />
    </CardHolder>
    <DigitalSignature />
    <Observation />
  </PrescriptionMessage>
</RxPrintedEvent>' , -- MessageContent - xml
          26 , -- PatientID - uniqueid
          16 , -- ProviderID - uniqueid
          NULL , -- UserID - uniqueid
          '' , -- Fault - varchar(max)
          '' , -- MessageRecipient - varchar(255)
          '9999140001156981' , -- MessageID - varchar(50)
          'RxPrintedEvent'  -- MessageEvent - varchar(255)
        )

SET @message = '<MessageID>' + CONVERT(VARCHAR(MAX), @@IDENTITY) + '</MessageID>'

-- Insert CSS_JOB_QUEUE
INSERT INTO dbo.CSS_JOB_QUEUE
        ( ORG_ID ,
          SITE_ID ,
          ACTV_IND ,
          JOB_TYPE_CD ,
          JOB_TYPE_PROP ,
          JOB_STATUS_CD ,
          JOB_PROGRESS_DESC ,
          SPOOLER_NM ,
          SPOOLER_DEVICE_NM ,
          USER_ID_REC_CREATE ,
          DESTINATION ,
          SERVER_NAME ,
          PRINTER_NAME ,
          CLIENT_STD_TZ ,
          JOB_PRIORITY ,
          RETRIES ,
          PAT_ID ,
          PHARM_ID ,
          RESUBMITTED ,
          DEPENDENCY_JOB_ID ,
          ORIG_JOB_ID ,
          PBM_ID ,
          PROCESS_AFTER_DT ,
          DATETIME_REC_CREATE ,
          USER_ID_LAST_MOD ,
          DATETIME_LAST_MOD ,
          TOTAL_RETRIES_COUNT
        )
VALUES  ( 0 , -- ORG_ID - int
          0 , -- SITE_ID - int
          'Y' , -- ACTV_IND - char(1)
          'OUT_SCR_PUB' , -- JOB_TYPE_CD - char(12)
          0 , -- JOB_TYPE_PROP - smallint
          0 , -- JOB_STATUS_CD - smallint
          '' , -- JOB_PROGRESS_DESC - varchar(5000)
          '' , -- SPOOLER_NM - varchar(64)
          '' , -- SPOOLER_DEVICE_NM - varchar(255)
          0 , -- USER_ID_REC_CREATE - numeric
          '' , -- DESTINATION - varchar(510)
          '' , -- SERVER_NAME - varchar(255)
          '' , -- PRINTER_NAME - varchar(255)
          '' , -- CLIENT_STD_TZ - varchar(32)
          1 , -- JOB_PRIORITY - int
          5 , -- RETRIES - smallint
          0 , -- PAT_ID - int
          0 , -- PHARM_ID - int
          0 , -- RESUBMITTED - tinyint
          0 , -- DEPENDENCY_JOB_ID - int
          0 , -- ORIG_JOB_ID - int
          0 , -- PBM_ID - int
          '2013-12-29 03:07:39' , -- PROCESS_AFTER_DT - datetime
          '2013-12-29 03:07:39' , -- DATETIME_REC_CREATE - datetime
          0 , -- USER_ID_LAST_MOD - numeric
          '2013-12-29 03:07:39' , -- DATETIME_LAST_MOD - datetime
          0  -- TOTAL_RETRIES_COUNT - int
        )
SET @jobID = @@IDENTITY

-- Insert CSS_JOB_QUEUE_ITEM
INSERT INTO dbo.CSS_JOB_QUEUE_ITEM
        ( ORG_ID ,
          SITE_ID ,
          JOB_ID ,
          ITEM_ID ,
          ITEM_ID_TYPE ,
          PROCESSING_TYPE ,
          PROCESSING_SUB_TYPE ,
          ITEM_STATUS ,
          TEMPLATE_FILENAME ,
          CONTROL_STRING ,
          EXPORT_FILENAME ,
          REQUEST ,
          USER_ID_REC_CREATE ,
          DATETIME_REC_CREATE ,
          USER_ID_LAST_MOD ,
          DATETIME_LAST_MOD
        )
VALUES  ( 0 , -- ORG_ID - int
          0 , -- SITE_ID - int
          @jobID , -- JOB_ID - uniqueid
          0 , -- ITEM_ID - numeric
          'OUT_SCR_PU' , -- ITEM_ID_TYPE - char(10)
          'OUT_SCR_PU' , -- PROCESSING_TYPE - char(10)
          '' , -- PROCESSING_SUB_TYPE - varchar(10)
          0 , -- ITEM_STATUS - smallint
          'Publish Outbound Message' , -- TEMPLATE_FILENAME - varchar(255)
          '' , -- CONTROL_STRING - varchar(255)
          '' , -- EXPORT_FILENAME - varchar(255)
          @message , -- REQUEST - varchar(8000)
          0 , -- USER_ID_REC_CREATE - numeric
          '2013-12-29 03:10:54' , -- DATETIME_REC_CREATE - datetime
          0 , -- USER_ID_LAST_MOD - numeric
          '2013-12-29 03:10:54'  -- DATETIME_LAST_MOD - datetime
        )
print @jobID
print @message
Rollback tran

