﻿using Ether.Core.Models.DTO;
using Ether.Core.Models.VSTS;
using System;

namespace Ether.Core.Models
{
    public class WorkItemResolution
    {
        private const string NoneValue = "None";
        private const string ErrorValue = "Error";

        public WorkItemResolution(VSTSWorkItem workitem, string resolution, string reason, DateTime resolutionDate, string memberEmail, string memberName)
        {
            WorkItemId = workitem.WorkItemId;
            WorkItemTitle = workitem.Title;
            WorkItemType = workitem.WorkItemType;

            Resolution = resolution;
            Reason = reason;
            ResolutionDate = resolutionDate;
            MemberEmail = memberEmail;
            MemberName = memberName;
        }

        private WorkItemResolution()
        {
        }

        public int WorkItemId { get; private set; }
        public string WorkItemTitle { get; private set; }
        public string WorkItemType { get; private set; }
        public string Resolution { get; private set; }
        public string Reason { get; private set; }
        public DateTime ResolutionDate { get; private set; }
        public string MemberEmail { get; private set; }
        public string MemberName { get; private set; }

        public bool IsNone => string.Equals(Resolution, NoneValue) && string.Equals(Reason, NoneValue);

        public bool IsError => WorkItemId == -1 && Resolution.Contains("Exception") && string.Equals(MemberEmail, ErrorValue) && string.Equals(MemberName, ErrorValue);

        public static WorkItemResolution None => new WorkItemResolution
        {
            WorkItemId = -1,
            WorkItemTitle = NoneValue,
            WorkItemType = NoneValue,

            Resolution = NoneValue,
            Reason = NoneValue,
            ResolutionDate = DateTime.MinValue,
            MemberEmail = NoneValue,
            MemberName = NoneValue,
        };

        public static WorkItemResolution GetError(Exception ex) => new WorkItemResolution
        {
            WorkItemId = -1,
            WorkItemTitle = ErrorValue,
            WorkItemType = ErrorValue,

            Resolution = ex.GetType().ToString(),
            Reason = ex.Message,
            ResolutionDate = DateTime.MinValue,
            MemberEmail = ErrorValue,
            MemberName = ErrorValue,
        };
    }
}
