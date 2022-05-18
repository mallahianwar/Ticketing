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
		setInterval(function () {

			$.ajax({
				type: "POST",
				data: {},
				url: receiveUrl,
				success: function (response) {
					if (response != null) {
						var messages = element.querySelector('[data-kt-element="messages"]');
						var messageOutTemplate = messages.querySelector('[data-kt-element="template-out"]');
						var messageInTemplate = messages.querySelector('[data-kt-element="template-in"]');
						var message;


						// Show example incoming message
						message = messageInTemplate.cloneNode(true);
						message.classList.remove('d-none');
						message.querySelector('[data-kt-element="message-text"]').innerText = response;
						messages.appendChild(message);
						messages.scrollTop = messages.scrollHeight;
					}

				},
				error: function (response) {

				}
			});


		}, 10000);
	}


	var handeMessaging = function(element) {
		var messages = element.querySelector('[data-kt-element="messages"]');
		var input = element.querySelector('[data-kt-element="input"]');

        if (input.value.length === 0 ) {
            return;
        }

		var messageOutTemplate = messages.querySelector('[data-kt-element="template-out"]');
		var messageInTemplate = messages.querySelector('[data-kt-element="template-in"]');
		var message;




		var msgstring = input.value;
		var user = $('[data-kt-element="send"]').data('userid');
		$.ajax({
			type: "POST",
			data: { message: msgstring, friend: user },
			url: sendUrl,
			success: function (response) {
				// Show example outgoing message
				message = messageOutTemplate.cloneNode(true);
				message.classList.remove('d-none');
				message.querySelector('[data-kt-element="message-text"]').innerText = input.value;
				input.value = '';
				messages.appendChild(message);
				messages.scrollTop = messages.scrollHeight;


				//var user = document.getElementById("userInput").value;
				//var message = document.getElementById("messageInput").value;
				//connection.invoke("SendMessage", user, msgstring).catch(function (err) {
				//	return console.error(err.toString());
				//});

			},
			error: function (response) {

			}
		});


		
		
		//setTimeout(function() {			
		//	// Show example incoming message
		//	message = messageInTemplate.cloneNode(true);			
		//	message.classList.remove('d-none');
		//	message.querySelector('[data-kt-element="message-text"]').innerText = 'Thank you for your awesome support!';
		//	messages.appendChild(message);
		//	messages.scrollTop = messages.scrollHeight;
		//}, 2000);
	}

	// Public methods
	return {
		init: function(element) {
			handeSend(element);
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
