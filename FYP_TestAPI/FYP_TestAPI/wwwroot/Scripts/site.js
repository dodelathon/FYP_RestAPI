const StatsAPI = "/api/DeviceData";
const ImageAPI = "/api/Image";
var StatsInterval;
var ImageInterval;

$(document).ready(function () {

	$(".User_Inputs #Interval_Selector").prop("selectedIndex", 4);
	LoadDevices();
	//getStats();
	ImageInterval = setInterval(refreshImage, 5000);
	StatsInterval = setInterval(getStats, 5000)

	$(".User_Inputs #Submit_Btn_Mainpage").click(function () {
		getStats()
	});

	$(".User_Inputs #Interval_Btn_Mainpage").click(function () {
		clearInterval(ImageInterval);
		clearInterval(StatsInterval);
		ImageInterval = setInterval(refreshImage, $(".User_Inputs #Interval_Selector").val());
		StatsInterval = setInterval(getStats, $(".User_Inputs #Interval_Selector").val());
	});
});



function getStats() {
	$.ajax({
		type: "GET",
		url: StatsAPI + "/GetStats",
		headers:
		{
			'Device': $(".User_Inputs #Device_Selector").val()
		},
		cache: false,
		success: function (data)
		{
			BuildTable(data);
		}
	});
}

function BuildTable(data) {
	const tBody = $(".Information_Display #Stats_Holder");

	$(tBody).empty();
	data = JSON.parse(data);
	$.each(data, function (key, item) {
		var tr = $("<tr></tr>").append($("<td></td>").text(key)).append($("<td></td>"));
		tBody.append(tr)
		$.each(item, function (Secondkey, Seconditem) {
			tr = $("<tr></tr>");
			if (typeof Seconditem == 'object') {
				if (Secondkey != "history") {
					tr.append($("<td></td>")).append($("<td></td>").text(Secondkey))
					tBody.append(tr);
					$.each(Seconditem, function (Thirdkey, Thirditem) {
						tr = $("<tr></tr>")
						tr.append($("<td></td>")).append($("<td></td>")).append($("<td></td>").text(Thirdkey))
							.append($("<td></td>").text(Thirditem))
						tBody.append(tr);
					});
				}
			}
			else {
				tr.append($("<td></td>")).append($("<td></td>").text(Secondkey))
					.append($("<td></td>").text(Seconditem))
				tBody.append(tr);
			}
		});
	});
}

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


function refreshImage() 
{
	$.ajax({
		type: "GET",
		url: ImageAPI + "/GetImage?Device=" + $(".User_Inputs #Device_Selector").val(),
		success: function (data) {
			const image = $(".Information_Display #Stream");
			//data = JSON.parse(data);
			image.attr("src", "data:image/jpeg;base64," + data.fileContents);
		}
	});
//	timestamp = (new Date()).getTime();
	//source += "&_=" + timestamp;
	//$(".Information_Display #Stream").attr("src", source);
	//console.log($(".Information_Display #Stream").attr("src"));
	//document.getElementById("Stream").src = source// + timestamp;
}
