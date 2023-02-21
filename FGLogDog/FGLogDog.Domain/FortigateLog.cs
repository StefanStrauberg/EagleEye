using System.Net;
using System;
using FGLogDog.Domain.Common;

namespace FGLogDog.Domain
{
    public class FortigateLog : BaseEntity
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public Guid LogId { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Level { get; set; }
        public string VD { get; set; }
        public int EventTime { get; set; }
        public IPAddress SrcIP { get; set; }
        public string SrcName { get; set; }
        public int SrcPort { get; set; }
        public string SrcIntf { get; set; }
        public string SrcIntfRole { get; set; }
        public IPAddress DstIP { get; set; }
        public string DstName { get; set; }
        public int DstPort { get; set; }
        public string DstIntf { get; set; }
        public string DstIntfRole { get; set; }
        public Guid PolUUID { get; set; }
        public int sessionid { get; set; }
        public int Proto { get; set; }
        public string Action { get; set; }
        public int PolicyId { get; set; }
        public string PolicyType { get; set; }
        public string PolicyMode { get; set; }
        public string Service { get; set; }
        public string DstCountry { get; set; }
        public string SrcCountry { get; set; }
        public string Trandisp { get; set; }
        public IPAddress TransIP { get; set; }
        public int TransPort { get; set; }
        public int AppId { get; set; }
        public string app { get; set; }
        public string AppCat { get; set; }
        public string AppRisk { get; set; }
        public int Duration { get; set; }
        public int SentByte { get; set; }
        public int RcvdByte { get; set; }
        public int SentPkt { get; set; }
        public int RcvdPkt { get; set; }
        public string UtmAction { get; set; }
        public int CountApp { get; set; }
        public string DevType { get; set; }
        public string OSName { get; set; }
        public string MasterSrcMAC { get; set; }
        public string SrcMAC { get; set; }
        public int SrcServer { get; set; }
    }
}