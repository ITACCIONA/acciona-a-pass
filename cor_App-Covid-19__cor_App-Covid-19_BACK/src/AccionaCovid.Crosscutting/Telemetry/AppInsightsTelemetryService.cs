using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Crosscutting
{
    public class AppInsightsTelemetryService : ITelemetryService
    {
        private readonly TelemetryClient telemetry;

        public AppInsightsTelemetryService(TelemetryClient telemetry)
        {
            this.telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
        }
        public void TrackEvent(string eventName)
        {
            telemetry.TrackEvent(eventName);
        }
    }
}
