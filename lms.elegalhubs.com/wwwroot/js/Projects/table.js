(() => { // webpackBootstrap
"use strict";
var __webpack_exports__ = {};
    var KTProjectsList = function () {
    // Define shared variables
    var table = document.getElementById('kt_table_projects');
    var datatable;
    var toolbarBase;
    var toolbarSelected;
    var selectedCount;

    // Private functions
    var initProjectTable = function () {
        // Set date data order
        const tableRows = table.querySelectorAll('tbody tr');
        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'order': [],
            "pageLength": 10,
            "lengthChange": false,
            'columnDefs': [
                { orderable: false, targets: 0 }, // Disable ordering on column 0 (checkbox)
                { orderable: false, targets: 5 }, // Disable ordering on column 6 (actions)                
            ]
        });

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        datatable.on('draw', function () {
            initToggleToolbar();
            handleDeleteRows();
            toggleToolbars();
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-projects-table-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Delete subscirption
    var handleDeleteRows = () => {
        // Select all delete buttons
        const deleteButtons = table.querySelectorAll('[data-kt-projects-table-filter="delete_row"]');
        deleteButtons.forEach(d => {
            // Delete button on click
            d.addEventListener('click', function (e) {
                e.preventDefault();
                const id = $(this).data("id");
                const href = $(this).attr("href");
                // Select parent row
                const parent = e.target.closest('tr');
                // Get project name
                const search_col = parent.querySelectorAll('td')[2].innerText;
                Confirm("Delete project", "warning", "Are you sure you want to delete " + search_col + " project ?",true,"",
                    function () {
                        $.ajax({
                            type: 'POST',
                            url: href,
                            data: { "id": id },
                            //dataType: 'json',
                            success: function (data) {
                                if (data.hasOwnProperty("message") && data.hasOwnProperty("color"))
                                    Confirm("", data.color, data.message, false, "ok", function () {
                                        // Remove current row
                                        if (data.color == "success")
                                            datatable.row($(parent)).remove().draw();
                                    }, function () {
                                        // Detect checked checkboxes
                                        toggleToolbars();
                                    })
                            },
                            error: function () {
                                Confirm("Error", "error", "Error ! the operation not succces , Please try again.", false, "OK", function () { }, function () { })
                            }
                        });
                    },
                    function () {
                        Confirm("Delete Canceled", "error", search_col + " was not deleted.", false, "OK", function () { }, function () { })
                    })
            })
        });
    }

    // Init toggle toolbar
    var initToggleToolbar = () => {
        // Toggle selected action toolbar
        // Select all checkboxes
        const checkboxes = table.querySelectorAll('[type="checkbox"]');

        // Select elements
        toolbarBase = document.querySelector('[data-kt-projects-table-toolbar="base"]');
        toolbarSelected = document.querySelector('[data-kt-projects-table-toolbar="selected"]');
        selectedCount = document.querySelector('[data-kt-projects-table-select="selected_count"]');
        const deleteSelected = document.querySelector('[data-kt-projects-table-select="delete_selected"]');

        // Toggle delete selected toolbar
        checkboxes.forEach(c => {
            // Checkbox on click event
            c.addEventListener('click', function () {
                setTimeout(function () {
                    toggleToolbars();
                }, 50);
            });
        });

        // Deleted selected rows
        deleteSelected.addEventListener('click', function () {
            // SweetAlert2 pop up --- official docs reference: https://sweetalert2.github.io/

            Confirm("Delete projects", "warning", "Are you sure you want to delete selected projects?", true, "",
                function () {
                    // Remove all selected customers
                    checkboxes.forEach(c => {
                        if (c.checked) {
                            var href = $(c.closest('tbody tr')).find('td a[data-kt-projects-table-filter="delete_row"]').attr("href")
                            var id = $(c.closest('tbody tr')).find('td a[data-kt-projects-table-filter="delete_row"]').data("id")
                            $.ajax({
                                type: 'POST',
                                url: href,
                                data: { "id": id },
                                //dataType: 'json',
                                success: function (data) {

                                    if (data.hasOwnProperty("message") && data.hasOwnProperty("color")) {
                                        if (data.color == "success")
                                            datatable.row($(c.closest('tbody tr'))).remove().draw();
                                        Confirm("", data.color, data.message, false, "ok",
                                            function () {
                                                // Remove current row
                                                if (data.color == "success") {
                                                    toggleToolbars(); // Detect checked checkboxes
                                                    initToggleToolbar(); // Re-init toolbar to recalculate checkboxes
                                                }
                                            },
                                            function () { }
                                        )
                                    }
                                },
                                error: function () {
                                    Confirm("Error", "error", "Error ! the operation not succces , Please try again.", false, "OK", function () { location.reload() }, function () { })
                                    
                                }
                            });

                        }
                    });
                    // Remove header checked box
                    const headerCheckbox = table.querySelectorAll('[type="checkbox"]')[0];
                    headerCheckbox.checked = false;
                },
                function () {
                    Confirm("Delete canceled", "error", "Selected projects was not deleted.", false, "Ok, got it!", function () { }, function () {})
                })


 
        });
    }
    // Toggle toolbars
        const toggleToolbars = () => {
        // Select refreshed checkbox DOM elements 
        const allCheckboxes = table.querySelectorAll('tbody [type="checkbox"]');

        // Detect checkboxes state & count
        let checkedState = false;
        let count = 0;

        // Count checked boxes
        allCheckboxes.forEach(c => {
            if (c.checked) {
                checkedState = true;
                count++;
            }
        });

        // Toggle toolbars
        if (checkedState) {
            selectedCount.innerHTML = count;
            toolbarBase.classList.add('d-none');
            toolbarSelected.classList.remove('d-none');
        } else {
            toolbarBase.classList.remove('d-none');
            toolbarSelected.classList.add('d-none');
        }
    }

    return {
        // Public functions  
        init: function () {
            if (!table) {
                return;
            }

            initProjectTable();
            initToggleToolbar();
            handleSearchDatatable();
            //handleResetForm();
            handleDeleteRows();
            //handleFilterDatatable();

        }
    }
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTProjectsList.init();
});
/******/ })()
;
//# sourceMappingURL=table.js.map