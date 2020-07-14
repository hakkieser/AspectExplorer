using AspectExplorer.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace AspectExplorer
{
    public class AspectExplorerLogAttributeModel
    {
        public string ResponseType { get; set; }
        public string ResponseArgsStr { get; set; }

        public string RequestTypes { get; set; }
        public string RequestArgsStr { get; set; }

        public string InvokeStackTrace { get; set; }
        public string InvokeMethodName { get; set; }
        public DateTime InvokeTime { get; set; }
    }

    public class AspectExplorerLogAttribute : AspectBase
    {
        public virtual void OnAfterlogProcess(RealTypeResponseArgument param, MethodContext _methodContext)
        {
            AspectExplorerLogAttributeModel _aspectLogModel = new AspectExplorerLogAttributeModel();
            _aspectLogModel.InvokeMethodName = _methodContext.MethodName;
            _aspectLogModel.InvokeStackTrace = "";
            _aspectLogModel.InvokeTime = DateTime.Now;
            if (param != null)
            {
                _aspectLogModel.ResponseType = param.Value.GetType().ToString();
            }

            // _aspectLogModel.RequestTypes = _methodContext.MethodBase.Args

            //
            try
            {
                StringBuilder sbRequestTypes = new StringBuilder();
                StringBuilder sbRequestArgs = new StringBuilder();
                foreach (var item in _methodContext.MethodBase.Args)
                {
                    sbRequestTypes.Append(item.GetType().ToString());

                    sbRequestArgs.Append(item.GetType().ToString()).Append(" : ");
                    foreach (PropertyInfo eachItem in item.GetType().GetProperties())
                    {
                        if (eachItem.GetValue(item) != null)
                        {
                            sbRequestArgs.Append(string.Format("{0} : {1}", eachItem.Name, eachItem.GetValue(item).ToString()));
                        }
                    }
                }
                _aspectLogModel.RequestTypes = sbRequestTypes.ToString();
                _aspectLogModel.RequestArgsStr = sbRequestArgs.ToString();
            }
            catch { }
            //
            try
            {
                if (param != null)
                {
                    StringBuilder sbResponseValue = new StringBuilder();
                    foreach (PropertyInfo item in param.Value.GetType().GetProperties())
                    {
                        if (item.GetValue(param.Value) != null)
                        {
                            sbResponseValue.Append(string.Format("{0} : {1}", item.Name, item.GetValue(param.Value).ToString()));
                        }
                    }
                    _aspectLogModel.ResponseArgsStr = sbResponseValue.ToString();
                }
            }
            catch { }
            //

            //
            try
            {
                StringBuilder sbJsonResult = new StringBuilder();
                sbJsonResult.Append(" # ").Append(DateTime.Now.ToString()).Append(" : ");
                sbJsonResult.Append(new JavaScriptSerializer().Serialize(_aspectLogModel));

                string _directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AspectExplorerLog/");
                if (!Directory.Exists(_directoryPath))
                {
                    Directory.CreateDirectory(_directoryPath);
                }
                File.AppendAllText(_directoryPath + DateTime.Now.ToString("yyyyMMdd") + "log.txt", sbJsonResult.ToString());
            }
            catch { }
        }
        public override void OnAfter(RealTypeResponseArgument param, MethodContext _methodContext)
        {
            this.OnAfterlogProcess(param.Value == null ? null : param, _methodContext);
        }

    }
}
