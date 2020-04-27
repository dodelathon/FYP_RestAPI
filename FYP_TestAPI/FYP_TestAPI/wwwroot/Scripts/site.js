const StatsAPI = "/api/DeviceData";
const ImageAPI = "/api/Image";
var AlertBool = false;
var StatsInterval;
var ImageInterval;
var Selected_Device

$(document).ready(function () {

	$(".User_Inputs #Interval_Selector").prop("selectedIndex", 4);
	LoadDevices();
	ImageInterval = setInterval(refreshImage, 5000);
	StatsInterval = setInterval(getStats, 5000);
	setInterval(LoadDevices(), 5000);

	//Provides the functionality for the device submission button.
	$(".User_Inputs #Device_Btn_Mainpage").click(function () {
		AlertBool = false;
		Selected_Device = $(".User_Inputs #Device_Selector").val();
		getStats();
	});
	// Gives the functionality to the Interval Button.
	$(".User_Inputs #Interval_Btn_Mainpage").click(function () {
		clearInterval(ImageInterval);
		clearInterval(StatsInterval);
		ImageInterval = setInterval(refreshImage, $(".User_Inputs #Interval_Selector").val());
		StatsInterval = setInterval(getStats, $(".User_Inputs #Interval_Selector").val());
	});
});


//Function attempts to get statistics for the selected device.
function getStats() {
	$.ajax({
		type: "GET",
		url: StatsAPI + "/GetStats",
		headers:
		{
			'Device': Selected_Device
		},
		cache: false,
		error: function (jqXHR, textStatus, errorThrown)
		{
			BuildTable(jqXHR.responseText, false);
		},
		success: function (data)
		{
			BuildTable(data, true);
		}
	});
}

//This function either builds the table containing the JSON information, 
//Or displays the error recieved from the server
function BuildTable(data, State) {
	const tBody = $(".Information_Display #Stats_Table");
	$(tBody).empty();

	if (State == true)
	{
		data = JSON.parse(data);
		$.each(data, function (key, item) {

			if (typeof item == 'object') {
				var tr = $("<tr></tr>").append($("<td></td>").text(key + ": ")).append($("<td></td>"));
				tBody.append(tr)
				$.each(item, function (Secondkey, Seconditem) {
					tr = $("<tr></tr>");
					if (typeof Seconditem == 'object') {
						if (Secondkey != "history") {
							tr.append($("<td></td>")).append($("<td></td>").text(Secondkey + ": "))
							tBody.append(tr);
							$.each(Seconditem, function (Thirdkey, Thirditem) {
								tr = $("<tr></tr>")
								tr.append($("<td></td>")).append($("<td></td>")).append($("<td></td>").text(Thirdkey + ": "))
									.append($("<td></td>").text(Thirditem))
								tBody.append(tr);
							});
						}
					}
					else {
						tr.append($("<td></td>")).append($("<td></td>").text(Secondkey + ": "))
							.append($("<td></td>").text(Seconditem));
						tBody.append(tr);
					}
				});
			}
			else {
				var tr = $("<tr></tr>").append($("<td></td>").text(key + ": ")).append($("<td></td>").text(item));
				tBody.append(tr)
			}
		});

	}
	else
	{
		var tr = $("<tr></tr>")
			.append($("<td></td>"))
			.append($("<h1></h1>").text(data));
		tBody.append(tr)
	}
}

//This function loads the Device selector dropdown with the registered devices on the server.
function LoadDevices() {
	$.ajax({
		type: "GET",
		url: StatsAPI + "/GetAllDevices",
		success: function (data) {
			const tBody = $(".User_Inputs #Device_Selector");
			
			$(tBody).empty();
			$.each(data, function (key, item)
			{
				tBody.append($("<option value=" + item.uuid + ">" + item.device_Name +"</option>"))
			});

		}
	});
}

/*
function addItem() {
	const item = {
		name: $("#add-name").val(),
		isComplete: false
	};

	$.ajax({
		type: "POST",
		accepts: "application/json",
		url: uri,
		contentType: "application/json",
		data: JSON.stringify(item),
		error: function (jqXHR, textStatus, errorThrown) {
			alert("Something went wrong!");
		},
		success: function (result) {
			getData();
			$("#add-name").val("");
		}
	});
}

function deleteItem(id) {
	$.ajax({
		url: uri + "/" + id,
		type: "DELETE",
		success: function (result) {
			getData();
		}
	});
}


$(".my-form").on("submit", function () {
	const item = {
		name: $("#edit-name").val(),
		isComplete: $("#edit-isComplete").is(":checked"),
		id: $("#edit-id").val()
	};

	$.ajax({
		url: uri + "/" + $("#edit-id").val(),
		type: "PUT",
		accepts: "application/json",
		contentType: "application/json",
		data: JSON.stringify(item),
		success: function (result) {
			getData();
		}
	});

	closeInput();
	return false;
});*/


// This function Displays the most recent image of a device, or alerts the user that no images have been sent.
function refreshImage() 
{
	$.ajax({
		type: "GET",
		url: ImageAPI + "/GetImage?Device=" + Selected_Device,
		error: function (jqXHR, textStatus, errorThrown){
			const image = $(".Information_Display #Stream");
			image.attr("src", "Stock_Image/3D_Printer.jpg");
			if (AlertBool == false) {
				alert(jqXHR.responseText + "\n" + "Image has been replaced with stock Image until an active source is selected!");
				AlertBool = true;
			}
		},
		success: function (data) {
			const image = $(".Information_Display #Stream");
			image.attr("src", "data:image/jpeg;base64," + data.fileContents);
			AlertBool = false;
		}
	});
}
