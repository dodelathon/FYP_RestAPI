﻿const uri = "https://donal-doherty.com/api/DeviceData";

$(document).ready(function () {
	LoadDevices();
	//getStats();
});

function getStats() {
	$.ajax({
		type: "GET",
		url: uri + "/GetStats",
		headers:
		{
			'Device': $("#Device_Selector").val()
		},
		cache: false,
		success: function (data)
		{
			const tBody = $("#Stats_Holder");

			$(tBody).empty();
			$.each(data, function (key, item) {
				const tr = $("<tr></tr>")
					.append($("<td></td>").text(key.name))
					.append($("<td></td>").text(item.name))

				tr.appendTo(tBody);
			});

			//todos= data;
		}
	});
}

function LoadDevices() {
	$.ajax({
		type: "GET",
		url: uri + "/GetAllDevices",
		success: function (data) {
			const tBody = $("#Device_Selector");

			//ta = $.parseJSON(data);
			$(tBody).empty();
			$.each(data, function (key, item)
			{
				tBody.append($("<option>"+ item +"</option>"))
			});

			//todos= data;
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


function refreshIt() 
{
	var source = "https://donal-doherty.com/api/image/GetImage?Device=" + $("#Device_Selector").val();
	timestamp = (new Date()).getTime();
	document.getElementById("Stream").src = source + timestamp;
	//setTimeout(refreshIt, 10000);
}
