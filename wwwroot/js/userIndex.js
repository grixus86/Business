$(document).ready(function () {
    $('#usersTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
        ],
        language: {
            "sDecimal": ",",
            "sEmptyTable": "No data available in the table",
            "sInfo": "_TOTAL_ from the recording _START_ - _END_ Showing records between",
            "sInfoEmpty": "No record",
            "sInfoFiltered": "(_MAX_ found in the registry)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "On Page _MENU_ Show Record",
            "sLoadingRecords": "Loading...",
            "sProcessing": "Processing...",
            "sSearch": "Search:",
            "sZeroRecords": "No matching records found",
            "oPaginate": {
                "sFirst": "First",
                "sLast": "End",
                "sNext": "Next",
                "sPrevious": "Back"
            },
            "oAria": {
                "sSortAscending": ": enable ascending column sort",
                "sSortDescending": ": enable descending column sort"
            },
            "select": {
                "rows": {
                    "_": "%d record selected",
                    "0": "",
                    "1": "1 record selected"
                }
            }
        }
    });
});