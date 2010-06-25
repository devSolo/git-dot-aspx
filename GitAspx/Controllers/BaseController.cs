#region License

// Copyright 2010 Jeremy Skinner (http://www.jeremyskinner.co.uk)
//  
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/git-dot-aspx

#endregion

namespace GitAspx.Controllers {
	using System.IO;
	using System.Web.Mvc;
	using GitAspx.Lib;

	public class BaseController : Controller {
		public AppSettings AppSettings { get; set; }

		protected string PktFlush() {
			return "0000";
		}

		protected string PktWrite(string input, params object[] args) {
			input = string.Format(input, args);
			return (input.Length + 4).ToString("X").PadLeft(4, '0') + input;
		}

		protected void WriteNoCache() {
			Response.AddHeader("Expires", "Fri, 01 Jan 1980 00:00:00 GMT");
			Response.AddHeader("Pragma", "no-cache");
			Response.AddHeader("Cache-Control", "no-cache, max-age=0, must-revalidate");
		}

		protected string ReadBody() {
			var reader = new StreamReader(Request.InputStream);
			return reader.ReadToEnd();
		}

		protected bool HasAccess(Rpc rpc, bool checkContentType = false) {
			if (checkContentType && Request.ContentType != string.Format("application/x-git-{0}-request", rpc.GetDescription())) {
				return false;
			}

			if (rpc == Rpc.ReceivePack) {
				return AppSettings.ReceivePack;
			}

			if (rpc == Rpc.UploadPack) {
				return AppSettings.UploadPack;
			}

			return false;
		}
	}
}