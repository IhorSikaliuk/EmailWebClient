$(function () {
	$("#loading").hide();
	$("#empty").hide();
	var page = 0
	var maxPages = 0
	$(document).ready(function () {
	    $('#loading').show();
	    $.ajax({
	        type: 'GET',
	        url: '/JS/GetMaxPages/',
	        success: function (data, textstatus) {
	            maxPages = parseInt(data);
	        }
	    });
		$.ajax({
			type: 'GET',
			url: '/JS/MailList/',
			success: function (data, textstasus) {
			    if (data != '' && page == 0) {
			        $("#loading").before(data);
					++page;
				}
			    else {
					$("#empty").show();
				}
				$("#loading").hide();
			},
			error: function (data, textstasus) {
				$("#loading").hide();
				$("#empty").show();
		}
		});
	});
});