/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
var __webpack_exports__ = {};
// Class definition
var KTAppChat = function () {
	// Private functions
	var handeSend = function (element) {
		if (!element) {
			return;
		}
		// Handle send
		KTUtil.on(element, '[data-kt-element="input"]', 'keydown', function(e) {
			if (e.keyCode == 13) {
				handeMessaging(element);
				e.preventDefault();

				return false;
			}
		});

		KTUtil.on(element, '[data-kt-element="send"]', 'click', function(e) {
			handeMessaging(element);
		});
	}


	var handeMessaging = function(element) {
		var messages = element.querySelector('[data-kt-element="messages"]');
		var input = element.querySelector('[data-kt-element="input"]');
		if (input.length === 0) {
            return;
        }
		var message;
		var msgstring = input.innerHTML;
		var receiver = $('[data-kt-element="send"]').data('userreceiver');
		var sender = $('[data-kt-element="send"]').data('usersender');
		$.ajax({
			type: "POST",
			data: { message: msgstring, friend: receiver },
			url: sendUrl,
			success: function (response) {
				if (receiver != "") {
					connection.invoke("SendMessageToGroup", sender, receiver, msgstring).catch(function (err) {
						return console.error(err.toString());
					});
				}
				else {
					connection.invoke("SendMessage", sender, msgstring).catch(function (err) {
						return console.error(err.toString());
					});
				}

				if (messages.querySelector('[data-kt-element="template-out"]')) {
					var messageOutTemplate = messages.querySelector('[data-kt-element="template-out"]');
					message = messageOutTemplate.cloneNode(true);
					message.classList.remove('d-none');
					message.querySelector('[data-kt-element="message-text"]').innerText = input.innerText;
					input.innerText = '';
					messages.appendChild(message);
					messages.scrollTop = messages.scrollHeight;
				} else
					location.reload();
			},
			error: function (response) {

			}
		});


	}

	var handeRecivedMsg = function () {
		//debugger;
		//var connection = new signalR.HubConnectionBuilder().withUrl("/messagehub").build();

		//connection.start();

		//connection.on("ReceiveMessageHandler", function (message, user) {
		//	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
		//	alert(user + " say " + msg);
		//});

		//connection.on("ReceiveMessage", function (user, message) {
		//	var messages = element.querySelector('[data-kt-element="messages"]');
		//	if (messages.data(userid) == user) {
		//		var messageOutTemplate = messages.querySelector('[data-kt-element="template-out"]');
		//		var message_node = messageOutTemplate.cloneNode(true);
		//		message_node.classList.remove('d-none');
		//		message_node.querySelector('[data-kt-element="message-text"]').innerText = message;
		//		input.value = '';
		//		messages.appendChild(message_node);
		//		messages.scrollTop = messages.scrollHeight;
  //          }


		//	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
		//	var encodedMsg = user + " says " + msg;
		//	alert(encodedMsg)
		//});
    }
	// Public methods
	return {
		init: function(element) {
			handeSend(element);
			handeRecivedMsg();
        }
	};
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
	// Init inline chat messenger
    KTAppChat.init(document.querySelector('#kt_chat_messenger'));

	// Init drawer chat messenger
	KTAppChat.init(document.querySelector('#kt_drawer_chat_messenger'));
});

/******/ })()
;
