//const { parse } = require("../demo1/assets/plugins/custom/vis-timeline/vis-timeline.bundle");

var err_msg = "There is a problem , please try again !";
var success_msg = "Successfuly";
var modalWrapper = "#OpenModal";

function getModalContent(sourceUrl, modalWrapper, Title) {
   
    $(modalWrapper).modal('show');
    $(modalWrapper + " .modal-header h2").html(Title);
    $(modalWrapper + " .modal-body").html("<h1>Loading .....</h1>");
    $.get(sourceUrl).done(function (result) {   
        //$.getScript("/demo1/assets/js/scripts.bundle.js").done(function (script, textStatus) {
        //    console.log("2finished loading and running test.js. with a status of" + textStatus);
        //});
        var parsed = $.parseHTML(result);        
        result = $(parsed).find(".kt_modal_content");      
        $(modalWrapper + " .modal-body").addClass("scroll-y");
        $(modalWrapper + " .modal-body").html(result);
         
        //$(modalWrapper, result).modal('show');
    }).always(function () {
        if ($("body").find(".image-input").length) {
            KTImageInput.createInstances();
        }
    });
}

$(document).on("click", ".openModal", function (e) {
    e.preventDefault();
    var title = $(this).attr("data-title");
    var href = $(this).attr("href");
    getModalContent(href, "#OpenModal", title);
    
});

function toast(type, message,title) {
    /* type = (error, info, warning, success) */
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "5000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
    };
    Command: toastr[type](message,title);
}

function Confirm(title, icon, text, showCancelButton, confirmButtonText, Confirmed, NotConfirme) {
    if (confirmButtonText == "") {
        confirmButtonText = "Yes, delete!"
    }
    Swal.fire({
        title: title,
        text: text,
        icon: icon,//warning ,success,error
        buttonsStyling: false,
        showCancelButton: showCancelButton,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: "<i class='far fa-check-circle'></i> " + confirmButtonText,
        cancelButtonText: '<i class="far fa-times-circle"></i> No, cancel!',
        customClass: {
            confirmButton: "btn fw-bold btn-danger",
            cancelButton: "btn btn-default"
        }
    }).then(function (result) {
        if (result.isConfirmed) {
            Confirmed()
        } else if (result.dismiss == 'cancel') {
            NotConfirme()
        }
    })
}

$(document).on("click", ".btn-save", function (event) {
    var _this = $(this);
    var form = _this.closest('form');
    if (form.find('.image-input-wrapper').length) {
        var m = form.find('.image-input-wrapper').css("backgroundImage").replace(/url\(|\)/g, "")
        if (m.indexOf('data:image') > -1) {
            m = m.split(',')[1];
            $("input[name=img]").val(m)
        }
    }
    var url = form.attr("action");
    var dataTosend = form.serialize();
    $.post(url, dataTosend).done(function (data) {
        var parsed = $.parseHTML(data)
        result = $(parsed).find(".kt_modal_content");

        if (result.length) {
            _this.closest('form').replaceWith(result);
        } else {
            if (data.hasOwnProperty("message") && data.hasOwnProperty("color"))
                Confirm("", data.color, data.message, false, "ok", function () { location.reload() }, function () { })
            $("#OpenModal").modal('hide');
        }
    }).done(function () {
        if ($('.data-table').length)         // use this if you are using id to check
        {
            //$(".data-table").draw();
        }
    });
});

function AjaxRequest(type, ajaxurl, formData, successCallback, errorCallback) {
    $.ajax({
        type: type,
        url: ajaxurl,
        data: formData,
        dataType: 'json',
        success: successCallback,
        error: errorCallback
    });
}

function isJson(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}

//$.ajaxSetup({
//    complete: function (result, textStatus) {
//        if (result.status == 200) {
//            if (isJson(result.responseText)) {
//                var data = JSON.parse(result.responseText);
//                if (data.hasOwnProperty("message")) {
//                    toast("success", data.message);
//                }
//            }
//        } else if (result.status == 422) {
//            //validation error
//        } else {
//            toast("error", err_msg);
//        }
//        //$.getScript("/demo1/assets/js/scripts.bundle.js").done(function (script, textStatus) {
//        //    //console.log("finished loading and running test.js. with a status of" + textStatus);
//        //});
//    }
//});

function FnDataTable(dtable) {
    dtable.DataTable();
    //var route = dtable.data('url');
    //var table = dtable.DataTable({
    //    processing: true,
    //    serverSide: true,
    //    ajax: route,
    //    language: {
    //        "paginate": {
    //            "first": "الأول",
    //            "last": "الأخير",
    //            "previous": "السابق",
    //            "next": "التالي"
    //        },
    //        "decimal": "",
    //        "emptyTable": "لا يوجد بيانات",
    //        "info": "عرض _START_ الى _END_ من _TOTAL_ عنصر ",
    //        "infoEmpty": "اظهار 0 ال 0 من 0 عنصر",
    //        "infoFiltered": "( تم البحث في _MAX_ عنصر )",
    //        "infoPostFix": "",
    //        "thousands": ",",
    //        "lengthMenu": "عرض _MENU_ من الصفوف",
    //        "loadingRecords": "جاري التحميل ، يرجى الانتظار.............",
    //        "processing": "جاري التنفيذ ....",
    //        "search": "بحث : ",
    //        "zeroRecords": "لم يتم العثور على نتائج",
    //        "aria": {
    //            "sortAscending": ": activate to sort column ascending",
    //            "sortDescending": ": activate to sort column descending"
    //        }
    //    }
    //});
}

//$(document).on('DOMSubtreeModified', '.validation-summary-errors', function () {
//    alert("done")
//})



