if ($("#ResetByDNI:checked").val() == "False") $("#dniControls").hide();

if ($("#ResetByDNI:checked").val() == "True") $("#emailControls").hide();

if ($("#ResetByDNI:checked").val() == undefined) { $("#dniControls").hide(); $("#emailControls").hide(); $("#submit").hide(); }

$("#ResetByDNI[value='False']").click(function () { $("#emailControls").show(); $("#dniControls").hide(); $("#submit").show(); });

$("#ResetByDNI[value='True']").click(function () { $("#emailControls").hide(); $("#dniControls").show(); $("#submit").show(); });