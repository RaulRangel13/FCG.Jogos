using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL.Errors;

namespace Domain._Core
{
    public abstract class ResultBaseCmd
    {

        public bool isFail { get; protected set; }

        public bool notFound { get; set; }

        public List<ErrorItemDetail> listOfErrors { get; private set; }


        public void AddError(ErrorItemDetail _error)
        {
            this.EnsureList();
            this.listOfErrors.Add(_error);
            this.MarkHasFail();
        }


        public void AddErrors(List<ErrorItemDetail> _errors)
        {
            this.EnsureList();
            this.listOfErrors.AddRange((IEnumerable<ErrorItemDetail>)_errors);
            this.MarkHasFail();
        }


        private void EnsureList()
        {
            if (this.listOfErrors != null)
                return;

            List<ErrorItemDetail> errorItemDetailList;
            this.listOfErrors = errorItemDetailList = new List<ErrorItemDetail>();
        }


        private void MarkHasFail() => this.isFail = true;

    }
}
