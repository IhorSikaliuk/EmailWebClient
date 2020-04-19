$(function () {
	$("#loading").hide();
	$("#empty").hide();
	var page = -1
	var maxPages = 0
    var _inCallback = false
	$(document).ready(function () {
	    $('#loading').show();
	    _inCallback = true;
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
			    if (data != '' && page < 0) {
			        $("#loading").before(data);
					++page;
				}
			    else {
					$("#empty").show();
				}
			    $("#loading").hide();
			    _inCallback = false;
			},
			error: function (data, textstasus) {
				$("#loading").hide();
				$("#empty").show();
				_inCallback = false;
		}
		});
	});

	function loadItems() {
	    if (page > -1 && page < maxPages - 1 && !_inCallback) {
	        _inCallback = true;
	        $('#loading').show();
	        $("#sidelist").scrollTop($("#sidelist").scrollTop() + 50);

	        $.ajax({
	            type: 'GET',
	            url: '/JS/MailList/?page=' + (page + 1),
	            success: function (data, textstasus) {
	                if (data != '') {
	                    $("#loading").before(data);
	                    ++page;
	                }
	                $("#loading").hide();
	                _inCallback = false;
	            },
	            error: function (data, textstasus) {
	                $("#loading").hide();
	                _inCallback = false;
	            }
	        });
	    }
	}

	$("#sidelist").scroll(function () {
	    if ($("#sidelist").scrollTop() >= ($(".message").length + $(".newmessage").length) * $(".message").height() - $("#sidelist").height()) {
	        loadItems();
	    }
    });

    $(document).ajaxStart(function () {
        $('body').addClass('wait');
    }).ajaxComplete(function () {
        $('body').removeClass('wait');
    });

});