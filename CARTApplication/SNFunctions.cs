using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CARTApplication
{
    public class SNProperty
    {
        #region ServiceNowReq


        private string _SNReqSubject;
        private string _SNReqFor;
        private string _SNReqOpenBy;
        private string _SNReqContactType;
        private string _SNReqState;
        private string _SNReqStage;
        private string _SNReqAssignedTo;
        private string _SNReqLocation;
        private string _SNReq_RequestState;
        private string _SNReq_Comments;
        private string _SNReq_SysID;
        private string _SNReq_Number;
        private string _SNReq_Approval;
        private string _SNReq_SubjectID;
        private string _SNReq_CallerID;
        private string _SNReq_u_person;
        private string _SNReq_AssignmentGroup;

        public string SNReqSubject { get { return _SNReqSubject; } set { _SNReqSubject = value; } }
        public string SNReqFor { get { return _SNReqFor; } set { _SNReqFor = value; } }
        public string SNReqOpenBy { get { return _SNReqOpenBy; } set { _SNReqOpenBy = value; } }
        public string SNReqContactType { get { return _SNReqContactType; } set { _SNReqContactType = value; } }
        public string SNReqState { get { return _SNReqState; } set { _SNReqState = value; } }
        public string SNReqStage { get { return _SNReqStage; } set { _SNReqStage = value; } }
        public string SNReqAssignedTo { get { return _SNReqAssignedTo; } set { _SNReqAssignedTo = value; } }
        public string SNReqLocation { get { return _SNReqLocation; } set { _SNReqLocation = value; } }
        
        public string SNReq_RequestState { get { return _SNReq_RequestState; } set { _SNReq_RequestState = value; } }
        public string SNReq_Comments { get { return _SNReq_Comments; } set { _SNReq_Comments = value; } }
        public string SNReq_SysID { get { return _SNReq_SysID; } set { _SNReq_SysID = value; } }
        public string SNReq_Number { get { return _SNReq_Number; } set { _SNReq_Number = value; } }
        public string SNReq_Approval { get { return _SNReq_Approval; } set { _SNReq_Approval = value; } }
        public string SNReq_SubjectID { get { return _SNReq_SubjectID; } set { _SNReq_SubjectID = value; } }
        public string SNReq_CallerID { get { return _SNReq_CallerID; } set { _SNReq_CallerID = value; } }
        public string SNReq_u_person { get { return _SNReq_u_person; } set { _SNReq_u_person = value; } }
        public string SNReq_AssignmentGroup { get { return _SNReq_AssignmentGroup; } set { _SNReq_AssignmentGroup = value; } }
        #endregion
    }

    public class SNFunctions
    {
        SNRequest.ServiceNow_sc_req_item soapClientRequest = new CARTApplication.SNRequest.ServiceNow_sc_req_item();
        System.Net.ICredentials cred;
        public string strSNWebUser;
        public string strSNWebPwd;
        string strErrorMailList = "";
        

        public SNFunctions()
        {
            strSNWebUser = ConfigurationManager.AppSettings["SNWebUser"];
            strSNWebPwd = ConfigurationManager.AppSettings["SNWebPwd"];

            cred = new System.Net.NetworkCredential(strSNWebUser, strSNWebPwd);
            soapClientRequest.Credentials = cred;
        }


        // For Service Now Request Insert
        public string SNReqInsert(SNProperty SNreq)
        {
            try
            {
                SNreq.SNReq_SysID = "";
                soapClientRequest.Credentials = cred;
                SNRequest.insert insert = new SNRequest.insert();

                insert.requested_for = SNreq.SNReqFor;
                insert.requested_by = SNreq.SNReq_u_person;
                insert.opened_by = SNreq.SNReqOpenBy;
                insert.short_description = SNreq.SNReqSubject;
                insert.u_subject_id = SNreq.SNReq_SubjectID;
                insert.contact_type = SNreq.SNReqContactType;
                insert.assignment_group = SNreq.SNReq_AssignmentGroup;
                insert.state = SNreq.SNReqState;
                insert.approval = SNreq.SNReq_Approval;
                insert.comments = SNreq.SNReq_Comments;
                insert.cmdb_ci = "CART";
                
                insert.active = true;
                SNRequest.get getreq = new CARTApplication.SNRequest.get();
                SNRequest.insertResponse resp = new SNRequest.insertResponse();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);
                resp = soapClientRequest.insert(insert);
                SNRequest.getRecords gets = new SNRequest.getRecords();
                gets.sys_id = resp.sys_id;
                SNreq.SNReq_SysID = resp.sys_id;
                SNRequest.getRecordsResponseGetRecordsResult[] resp1;
                resp1 = soapClientRequest.getRecords(gets);
                SNreq.SNReq_Number = resp1[0].number.ToString();
                return SNreq.SNReq_Number;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
