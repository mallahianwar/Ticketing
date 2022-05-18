"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/messagehub").build();
connection.start();
connection.on("ReceiveMessageHandler", function (message,user) {
	//alert(user + " say " + message);
	toast("info", message, "New Notification")
	var notif = parseInt($(".notification-badge").text().trim())
	$(".notification-badge").text(notif + 1)
});

connection.on("ReceiveMQMessageHandler", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    alert("MQ" + msg);
});

connection.on("ReceiveMessage", function (user, message) {
	debugger;
	if ($('[data-kt-element="messages"]').length)
		var messages = $('[data-kt-element="messages"]');
	else
		return;
	if (messages.data('userid') == user) {
		if (messages.find('[data-kt-element="template-in"]').length) {
			var messageInTemplate = messages.find('[data-kt-element="template-in"]')[0];
			var message_node = messageInTemplate.cloneNode(true);
			message_node.classList.remove('d-none');
			$(message_node).find('[data-kt-element="message-text"]').text($(message).text());
			messages.append($(message_node).html());
			messages.scrollTop(messages.prop('scrollHeight'));
		} else {
			location.reload();
        }		
	}
});
//connection.on("ReceiveMessage", function(user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;"    place(/>/g, "&gt;");
//    var encodedMs    alert(encodedMsg) msg;
//    alert(encodedMsg)

//});