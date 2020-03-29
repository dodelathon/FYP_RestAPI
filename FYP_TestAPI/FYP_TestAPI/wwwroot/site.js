const uri = "https://donal-doherty.com/api/DeviceData";

$(document).ready(function () {
	LoadDevices();
	//getStats();
	$("#Submit_Btn_Mainpage").click(function () {
		getStats()
	});
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
			//BuildTable(data);
			TestTable(data);
		}
	});
}

function TestTable(data) {
	const tBody = $("#Stats_Holder");
	$(tBody).empty();
	data = JSON.parse(data);
	$.each(data, function (key, item) {
		const tr = $("<tr></tr>")
			.append(
				$("<td></td>").text(key)
		).append(
			$("<tr></tr>")
		)
		tr.appendTo(tBody);
	});
}

function BuildTable(data) {
	const tBody = $("#Stats_Holder");

	$(tBody).empty();
	data = JSON.parse(data);
	var x = 0;
	$.each(data, function (key, item) {
		console.log(x);
		x += 1;
		const tr = $("<tr></tr>").append($("<td></td>").text(key));
		$.each(item, function (Secondkey, Seconditem) {
			if (typeof Seconditem == 'object') {
				//console.log(typeof Seconditem);
				//console.log(Secondkey);
				if (Secondkey != "history") {
					tr.append($("<td></td>").text(Secondkey))
					$.each(Seconditem, function (Thirdkey, Thirditem) {
						tr.append($("<td></td>").text(Thirdkey))
							.append($("<td></td>").text(Thirditem))
					});
				}
			}
			else {
				//console.log("Not a string");
				tr.append($("<td></td>").text(Secondkey))
					.append($("<td></td>").text(Seconditem))
			}

		});
		//tBody.append($("</tr>"));

	});
	tBody.append(tr);
}

function LoadDevices() {
	$.ajax({
		type: "GET",
		url: uri + "/GetAllDevices",
		success: function (data) {
			const tBody = $("#Device_Selector");

			
			$(tBody).empty();
			$.each(data, function (key, item)
			{
				tBody.append($("<option value=" + item.uuid + ">" + item.device_Name +"</option>"))
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
	console.log("Here " + $("#Device_Selector").val());
	var source = "https://donal-doherty.com/api/image/GetImage?Device=" + $("#Device_Selector").val();
	timestamp = (new Date()).getTime();
	document.getElementById("Stream").src = source// + timestamp;
	//setTimeout(refreshIt, 10000);
}
