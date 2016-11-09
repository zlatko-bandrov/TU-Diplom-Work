namespace LottoDemo.LottoGeneratorService
{
    partial class LottoGenerator
    {
        private System.ComponentModel.IContainer components = null;
        private System.Diagnostics.EventLog eventLog;

        private string EventLogName = "LottoDemoEventLog";
        private string EventLogSourceName = "LottoDemoEventSource";

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServiceName = "LottoDemoService";

            components = new System.ComponentModel.Container();

            this.eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(this.EventLogSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(this.EventLogSourceName, this.EventLogName);
            }
            this.eventLog.Source = this.EventLogSourceName;
            this.eventLog.Log = this.EventLogName;
        }

        #endregion
    }
}
