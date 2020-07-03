using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Crosscutting
{
    public interface ITelemetryService
    {
        void TrackEvent(string eventName);
    }
}
