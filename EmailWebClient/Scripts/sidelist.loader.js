$(function () {
	$("#loading").hide();
	$("#empty").hide();
	var page = 0
	$(document).ready(function () {
		$('#loading').show();
		$.ajax({
			type: 'GET',
			url: '/Home/MailList',
			success: function (data, textstasus) {
				if (data != '' && page == 0) {
					$("#sidelist").append(data);
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