using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CoreFramework.Logging.BO;

namespace SPLibrary.CoreFramework.WebConfigInfo
{
    public sealed class CustomerPrincipal : ICustomerPrincipal
    {
        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        public CustomerProfile CustomerProfile
        {
            get
            {
                //LogBO _log = new LogBO(typeof(CustomerProfile));
                
                //_log.Info("step 1");

                int customerId;
                CustomerProfile up = null;
                if (Equals(CustomerPrincipal.SessionProfile, null) || string.IsNullOrEmpty(SessionProfile.CustomerAccount))
                {
                    //_log.Info("step 2");
                    if (!Equals(HttpContext.Current, null))
                    {
                        //_log.Info("step 3");
                        int.TryParse(HttpContext.Current.User.Identity.Name.Replace("SPCustomer_", ""), out customerId);
                    }
                    else
                    {
                        //For TaskEngine module 
                        customerId = 1;
                    }


                    up = new CustomerProfile();
                    
                    if (customerId > 0){
                        //CustomerViewVO uVO = uDAO.FindById(customerId);

                        //string url = ConfigInfo.Instance.APIURL;
                        string token = "";
                        if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                        {
                            token = "";
                            return up;
                        }
                        //else
                        //{
                        //    token = HttpContext.Current.Session["#Session#TOKEN"].ToString();
                        //}

                        //HttpWebRequest request = WebRequest.Create(url + "/SPWebAPI/Customer/GetCustomer?customerId=" + customerId + "&token=" + token) as HttpWebRequest;

                        //string resultStr = "";
                        //// Get response
                        //try
                        //{
                        //    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                        //    {
                        //        // Get the response stream
                        //        StreamReader reader = new StreamReader(response.GetResponseStream());

                        //        // Console application output
                        //        resultStr = reader.ReadToEnd();
                        //    }
                        //}catch
                        //{
                        //    return up;
                        //}
                        //ResultObject result = JsonConvert.DeserializeObject<ResultObject>(resultStr);

                        //CustomerViewVO uVO = JsonConvert.DeserializeObject<CustomerViewVO>(result.Result.ToString());
                        //_log.Info("step 4");
                        CustomerBO uBO = new CustomerBO(new CustomerProfile());
                        CustomerViewVO uVO = uBO.FindById(customerId);                        

                        up.CustomerId = uVO.CustomerId;
                        up.CustomerAccount = uVO.CustomerAccount;
                        up.CustomerName = uVO.CustomerName;
                        //up.BusinessId = uVO.BusinessId;
                        //up.AgencyId = uVO.AgencyId;
                    }
                    if (!string.IsNullOrEmpty(up.CustomerAccount))
                    {
                        CustomerPrincipal.SessionProfile = up;
                    }
                }
                else
                {
                   // _log.Info("step 5");
                    up = CustomerPrincipal.SessionProfile;
                }
                return up;
            }
        }

        /// <summary>
        /// Get the state of current user login.
        /// </summary>
        public bool IsCustomerLoginIn
        {
            get { return (CustomerProfile != null && !string.IsNullOrEmpty(CustomerProfile.CustomerAccount)) ? true : false; }
        }

        /// <summary>
        /// Gets or sets the session profile.
        /// </summary>
        /// <value>The session profile.</value>
        private static CustomerProfile SessionProfile
        {
            get
            {

                if (HttpContext.Current == null || HttpContext.Current.Session["#Customer#Session#P#r#ofile"] == null)
                {
                    return null;

                }
                else
                {
                    return HttpContext.Current.Session["#Customer#Session#P#r#ofile"] as CustomerProfile;
                }

            }
            set
            {
                if (!Equals(HttpContext.Current, null))
                {
                    HttpContext.Current.Session["#Customer#Session#P#r#ofile"] = value;
                }
            }
        }
        
        /// <summary>
        /// Clears the profile session.
        /// </summary>
        public static void ClearProfileSession()
        {
            SessionProfile = new CustomerProfile();
            SessionProfile = null;
        }
    }
}
