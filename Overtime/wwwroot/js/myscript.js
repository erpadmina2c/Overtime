$(document).ready(function () {
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();
    var eventsdata = new Array;

    $("#rq_cre_by").select2({ width: '100%' });
    $("#rq_dep_id").select2({ width: '100%' });
    $("#rq_cre_for").select2({ width: '100%' });
    $("#us_u_id").select2({ width: '100%' });
    $("#User").select2({ width: '100%' });
    $("#ud_user_id").select2({ width: '100%' });
    $("#user").select2({ width: '100%' });
    $("#tr_u_id").select2({ width: '100%' });
    $("#urh_u_id").select2({ width: '100%' });
    $("#urh_reporting_to").select2({ width: '100%' });
    $("#ud_depart_id").select2({ width: '100%' });
    $("#u_role_id").select2({ width: '100%' });
    $('#saveMenu').click(function () {
      
        var reqRow = [];
        $("#multiselect_to option").each(function () {
            reqRow.push($(this).val());
        });

        var value = $('#SelectRole').val();
        if (value == 0) {
            alert("Please Choose Role");
        } else {
            var type = $('input[name=frmTypes]:checked').val();
            var data = new FormData();

            data.append('role', value);
            data.append('type', type);
            data.append('menus', reqRow);
            $.ajax({
                url: "/RoleMenu/saveMenuitems",
                type: "POST",
                contentType: false,
                processData: false,
                cache: false,
                data: data,
                success: function (response) {
                    alert(response);
                    $("#multiselect_to").empty();
                    $("#multiselect").empty();
                },
                error: function () {
                    $("#multiselect_to").empty();
                    $("#multiselect").empty();

                }
            });
        }
    });
    function getDaysInMonth(month, year) {
        return new Date(year, month, 0).getDate();
    }
    if ($("#calendar").length) {
        
        var date = new Date();
        var d = date.getDate(),
            m = date.getMonth(),
            y = date.getFullYear();
        var evts = [];
        for (var i = 1; i <= getDaysInMonth(m, y); i++) {
            evts.push({
                title: "John",
                start: new Date(y, m, i),
                backgroundColor: '#f56954',
                borderColor: '#f56954'
            });
        }
    }

    $("#SelectRole").change(function () {
        load_user_menus();
    });
    $("#multiselect_rightSelected").click(function () {
        if ($("#multiselect_to option[value='" + $("#multiselect").val()+"']").length ==0) {
            $("#multiselect_to").append($("<option />").val($("#multiselect option:selected").val()).text($("#multiselect option:selected").text()));
            $("#multiselect option[value='" + $("#multiselect option:selected").val()+"']").remove();
        }
    });
    $("#multiselect_rightAll").click(function () {
        if ($('#multiselect option').length != 0) {
            $("#multiselect_to").empty();
            $("#multiselect option").each(function () {

                if ($("#multiselect_to option[value='" + $(this).val() + "']").length == 0) {
                    $("#multiselect_to").append($("<option />").val($(this).val()).text($(this).text()));
                    $("#multiselect option[value='" + $(this).val() + "']").remove();
                }
            });
        }    
    });
    $("#multiselect_leftSelected").click(function () {
        if ($("#multiselect option[value='" + $("#multiselect_to").val() + "']").length == 0) {
           
            $("#multiselect").append($("<option />").val($("#multiselect_to option:selected").val()).text($("#multiselect_to option:selected").text()));
            $("#multiselect_to option[value='" + $("#multiselect_to option:selected").val() + "']").remove();
        }
    });
    $("#multiselect_leftAll").click(function () {
        if ($('#multiselect_to option').length != 0) {
            $("#multiselect").empty();
            $("#multiselect_to option").each(function () {

                if ($("#multiselect option[value='" + $(this).val() + "']").length == 0) {
                    $("#multiselect").append($("<option />").val($(this).val()).text($(this).text()));
                    $("#multiselect_to option[value='" + $(this).val() + "']").remove();
                }
            });
        }
    });
    $('.multiselect').multiselect();
    $('#forWhom').change(function() {
        if (!$(this).is(":checked")) {
            var returnVal = confirm("Are you sure?");
            $(this).attr("uncheck", returnVal);
        }
        else {
            alert();
        }
              
    });
    $('[data-toggle="tooltip"]').tooltip(); 
    if ($("#demo").length) {
        setInterval(myTimer, 1000);
    }
    if ($("#liveMonitoring").length) {
       setInterval(loadTimeForAllTr, 1000);
        //loadTimeForAllTr();
    }
    if ($("#mytable").length) {
        $('#mytable').DataTable({
            dom: 'lBfrtip',
            buttons: [
                'copyHtml5',
                'excelHtml5',
                'pdfHtml5'
            ]
         } );
    }
    if ($("#table2").length) {
        $('#table2').DataTable({
            dom: 'lBfrtip',
            buttons: [
                'copyHtml5',
                'excelHtml5',
                'pdfHtml5'
            ]
        });
    }
    
    $("#name").autocomplete({

        source: function (req, resp) {
            var data = new FormData();
            data.append('name', $("#name").val());
            $.ajax({
                url: "/OvertimeRequest/UsersName",
                type: "POST",
                contentType: false,
                processData: false,
                cache: false,
                data: data,
                success: function (data) {
                    resp(data);
                },
                error: function () {
                }
            });
        },
        select: function (event, ui) {

            var TABKEY = 9;
            this.value = ui.item.value;

            if (event.keyCode == TABKEY) {
                event.preventDefault();
                $('#name').focus();
            }

            return false;
        },
        position: {
            offset: '1000 4' // Shift 20px to the left, 4px down.
        }
    });
   
    $('input[type=radio][name=frmTypes]').change(function () {

        load_user_menus();
    });
    $('#role').change(function () {
        load_user_menus()
    });
    $('#external-events div.external-event').each(function () {
 
        // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
        // it doesn't need to have a start or end
        var eventObject = {
            title: $.trim($(this).text()) // use the element's text as the event title
        };

        // store the Event Object in the DOM element so we can get to it later
        $(this).data('eventObject', eventObject);

        // make the event draggable using jQuery UI
        $(this).draggable({
            zIndex: 999,
            revert: true,      // will cause the event to go back to its
            revertDuration: 0  //  original position after the drag
        });

    });


    /* initialize the calendar
    -----------------------------------------------------------------*/

    var calendar = $('#calendar').fullCalendar({
        header: {
            left: 'title',
            center: 'month',
            right: 'prevYear,prev,next,nextYear today'
        },
        editable: true,
        firstDay: 1, //  1(Monday) this can be changed to 0(Sunday) for the USA system
        selectable: true,
        defaultView: 'month',

        axisFormat: 'h:mm',
        columnFormat: {
            month: 'ddd',    // Mon
            week: 'ddd d', // Mon 7
            day: 'dddd M/d',  // Monday 9/7
            agendaDay: 'dddd d'
        },
        titleFormat: {
            month: 'MMMM yyyy', // September 2009
            week: "MMMM yyyy", // September 2009
            day: 'MMMM yyyy'                  // Tuesday, Sep 8, 2009
        },
        allDaySlot: true,
        selectHelper: true,
        select: function (start, end, allDay) {
            var title = prompt('Event Title:');
            if (title) {
                calendar.fullCalendar('renderEvent',
                    {
                        title: title,
                        start: start,
                        end: end,
                        allDay: allDay
                    },
                    true // make the event "stick"
                );
            }
            calendar.fullCalendar('unselect');
        },
        droppable: true, // this allows things to be dropped onto the calendar !!!
        drop: function (date, allDay) { // this function is called when something is dropped
           
            // retrieve the dropped element's stored Event Object
            var originalEventObject = $(this).data('eventObject');

            // we need to copy it, so that multiple events don't have a reference to the same object
            var copiedEventObject = $.extend({}, originalEventObject);

            // assign it the date that was reported
            copiedEventObject.start = date;
            copiedEventObject.allDay = allDay;

            // render the event on the calendar
            // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
            $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

            // is the "remove after drop" checkbox checked?
            if ($('#drop-remove').is(':checked')) {
                // if so, remove the element from the "Draggable Events" list
                $(this).remove();
            }

        },
        events: evts,
        buttonText: {
            prevYear: "<<",
            nextYear: ">>",
            prev:"<",
            next:">"
        },
        
       
    });


});

function openWorkflowDetl(id) {
        var data = new FormData();
        data.append('ID', id);
        $.ajax({
            url: "/Workflow/Details",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                $("#modalContainer").html(response);
                $("#wd_workflow_id").val(id);
                $('#myModal').modal('show');

            },
            error: function () {
            }
        });
}

function saveWorkFlowDetails() {
    var workflowid = $("#wd_workflow_id").val();
    var role_id = $("#wd_role_id").val();
    var priority = $("#wd_priority").val();

    var data = new FormData();
    data.append('wd_workflow_id', workflowid);
    data.append('wd_role_id', role_id);
    data.append('wd_priority', priority);
    $.ajax({
        url: "/WorkflowDetail/Create",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#modalContainer").html(response);
            $('#myModal').modal('show');
            $("#wd_priority").val("");

        },
        error: function () {
        }
    });
}


function overTimeRequestReport() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("rq_dep_id", $("#rq_dep_id").val());
    data.append("reportrange", $("#reportrange").val());
    data.append("rq_cre_by", $("#rq_cre_by").val());
    data.append("rq_cre_date", $("#rq_cre_date").val());
    $.ajax({
        url: "/OvertimeRequest/CustomReport",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            $('#mytable').DataTable({
                dom: 'lBfrtip',
                buttons: [
                    'copyHtml5',
                    'excelHtml5',
                    'pdfHtml5'
                ]
            });
            $('#overlay').fadeOut()
        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}

function GetDailyAttendance() {
    if ($("#c_date").val() == "") {
        alert('select Date');
        return;
    }
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("date", $("#c_date").val());
    $.ajax({
        url: "/Attendance/GetDailyAttendanceByDate",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            $('#mytable').DataTable({
                dom: 'lBfrtip',
                buttons: [
                    'copyHtml5',
                    'excelHtml5',
                    'pdfHtml5'
                ]
            });
            $('#overlay').fadeOut()
        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}

function GetMonthReport() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("reportrange", $("#attdatarange").val());
    $.ajax({
        url: "/Attendance/GetMonthReport",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            $('#mytable').DataTable({
                dom: 'lBfrtip',
                buttons: [
                    'copyHtml5',
                    'excelHtml5',
                    'pdfHtml5'
                ]
            });
            $('#overlay').fadeOut()
        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}

function workflowHistory(rowid,doc_id,workflow,status) {
    var data = new FormData();
    data.append("rowid", rowid);
    data.append("doc_id", doc_id);
    data.append("workflow", workflow);
    
    $.ajax({
        url: "/WorkflowTracker/History",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            workflowDetailStatus(workflow, status);
            $("#modalContainer").html(response);
            $('#historyModal').modal('show');

        },
        error: function () {
        }
    });
}

function workflowDetailStatus( workflow,status) {
    var data = new FormData();
    data.append("workflow", workflow);
    data.append("status", status); 
    $.ajax({
        url: "/WorkflowDetail/Status",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#cont").html(response);
          
        },
        error: function () {
        }
    });
}

function OverTimeConsolidatedReport() {
    var data = new FormData();
    data.append("rq_dep_id", $("#rq_dep_id").val());
    data.append("rq_cre_for", $("#rq_cre_for").val());
    data.append("reportrange", $("#reportrange").val());
    $.ajax({
        url: "/OvertimeRequest/ConsolidatedReports",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            $('#mytable').DataTable({
                dom: 'lBfrtip',
                buttons: [
                    'copyHtml5',
                    'excelHtml5',
                    'pdfHtml5'
                ]
            });
        },
        error: function () {
        }
    });
}
$(function () {

    var start = moment().subtract(29, 'days');
    var end = moment();

    function cb(start, end) {
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);

    cb(start, end);

});

$(function () {

    var start = moment().subtract(29, 'days');
    var end = moment();

    function cb(start, end) {
        $('#attdatarange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    }

    $('#attdatarange').daterangepicker({
        startDate: start,
        endDate: end,
        ranges: {
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);

    cb(start, end);

});



function myTimer() {
 var date1 = new Date();
  //  var date2 = Date.parse("2020/05/29 21:59:00");
    var date2 = Date.parse($("#lbl_start").text());
var delta = Math.abs(date1 - date2) / 1000;

// calculate (and subtract) whole days
var days = Math.floor(delta / 86400);
delta -= days * 86400;

// calculate (and subtract) whole hours
var hours = Math.floor(delta / 3600) % 24;
delta -= hours * 3600;

// calculate (and subtract) whole minutes
var minutes = Math.floor(delta / 60) % 60;
delta -= minutes * 60;

// what's left is seconds
var seconds = delta % 60;
    document.getElementById("demo").innerHTML = days + ": " + hours + ":" + minutes + ":" + Math.round(seconds);
}


function loadTimeForAllTr() {
    if ($("#mytable").length) {
        
        $("tr.a").each(function (i, tr) {
            var value = $(this).find("input.b").val();
           
            var date1 = new Date();
            //  var date2 = Date.parse("2020/05/29 21:59:00");
            var date2 = Date.parse($(this).find("#starttime" + value).val() );
            var delta = Math.abs(date1 - date2) / 1000;

            // calculate (and subtract) whole days
            var days = Math.floor(delta / 86400);
            delta -= days * 86400;

            // calculate (and subtract) whole hours
            var hours = Math.floor(delta / 3600) % 24;
            delta -= hours * 3600;

            // calculate (and subtract) whole minutes
            var minutes = Math.floor(delta / 60) % 60;
            delta -= minutes * 60;

            // what's left is seconds
            var seconds = delta % 60;
            $(this).find("#Duration" + value).text(n(days) + ": " + n(hours) + ":" + n(minutes) + ":" + n(Math.round(seconds)));
        });
    }

}

function n(n) {
    return n > 9 ? "" + n : "0" + n;
}

function holdHistory(rowid, doc_id,from) {
    var data = new FormData();
    data.append("rowid", rowid);
    data.append("doc_id", doc_id);
    data.append("from", from);

    $.ajax({
        url: "/Hold/History",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#modalContainer").html(response);
            $('#historyModal').modal('show');
        },
        error: function () {
        }
    });
}
function replayForHold(id) {
    var data = new FormData();
    data.append("id", id);
    data.append("replay", $("#replay" + id).val().replace(/[\n\r]/g, ''));
    $.ajax({
        url: "/Hold/Replay",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
        },
        error: function () {
        }
    });
}
function hold(id) {
    var data = prompt("Reason For Blocking?? ", "");
    if (data != null) {
        $("#reason" + id).val(data);
        $("#hold" + id).submit();
    }
   
}
function unhold(id) {
    var data = prompt("Reason For Blocking?? ", "");
    if (data != null) {
        $("#reason" + id).val(data);
        $("#unhold" + id).submit();
    }
   
}

function load_user_menus() {
    var $option = $('#SelectRole').find('option:selected');
    var role = $option.val();

    var type = $('input[name=frmTypes]:checked').val();
    var data = new FormData();
    data.append('role', role);
    data.append('type', type);
    $.ajax({
        url: "/RoleMenu/showRoleMenus",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            var data = JSON.parse(response);
            var i;
            if ($.isEmptyObject(response)) {
                $("#multiselect_to").empty();
                $("#multiselect").empty();
            } else {
              
                $('#saveMenu').prop('disabled', false);
                $("#multiselect_to").empty();
                $("#multiselect").empty();
                for (i = 0; i < data.userMenu.length; ++i) {
                    $("#multiselect_to").append($("<option />").val(data.userMenu[i].m_id).text(data.userMenu[i].m_description));
                   
                }
                for (i = 0; i < data.allMenu.length; ++i) {
                    $("#multiselect").append($("<option />").val(data.allMenu[i].m_id).text(data.allMenu[i].m_description));

                }
            }
        },
        error: function () {
            
            $("#multiselect_to").empty();
            $("#multiselect").empty();
        }
    });
}

function viewInsights(id,doc_id,flag) {
    var data = new FormData();
    data.append('id', id);
    data.append('doc_id', doc_id);
    $.ajax({
        url: "/Insights/index",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#insightsContainer").html(response);
            if (flag == 0) {
                $("#filter").empty()
            }
            $("#id").val(id);
            $("#doc_id").val(doc_id);
            $('#insightsModal').modal('show');
        },
        error: function () {
        }
    });
}



function saveInsight() {
    var id = $("#id").val();
    var doc_id = $("#doc_id").val();
    var in_remarks = $("#remarks").val();

    var data = new FormData();
    data.append('id', id);
    data.append('doc_id', doc_id);
    data.append('remarks', in_remarks);
    $.ajax({
        url: "/Insights/Create",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#insightsContainer").html(response);
            $("#id").val(id);
            $("#doc_id").val(doc_id);
            $('#insightsModal').modal('show');
            $("#remarks").val("");
            window.location.reload();
        },
        error: function () {
        }
    });
}

function Reject(id) {
    var data = prompt("Reason For Rejection?? ", "");
    if (data != null && data != "") {
        $("#reason" + id).val(data);
        $("#reject" + id).submit();
    }
    else {
        alert('please enter Reason!!!');
    }
}
function updateHour() {

    if ($("#time").val() != 0) {
        $("#wh_hours").val(parseFloat($("#time").val()/60).toFixed(2));
    }
   
    
}

function consolidatedByType() {
    var data = new FormData();
    data.append("type", $("#type").val());
    if ($("#type").val() != "") {
        data.append("reportrange", $("#reportrange").val());
        $.ajax({
            url: "/OvertimeRequest/consolidateReportByType",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                $("#container").html(response);

                if ($("#type").val() == "Department") {
                    $('td[name^="emp"]').remove();
                    $('th[name^="emp"]').remove();
                }
                else {
                    $('td[name^="dep"]').remove();
                    $('th[name^="dep"]').remove();
                }
                $('#mytable').DataTable({
                    dom: 'lBfrtip',
                    buttons: [
                        'copyHtml5',
                        'excelHtml5',
                        'pdfHtml5'
                    ]
                });
            },
            error: function () {
            }
        });
    } else {
        alert("Please choose Report type");
    }
    
}

function Workinghour(id, doc_id, flag) {
   
    var data = new FormData();
    data.append('id', id);
    data.append('doc_id', doc_id);
    $.ajax({
        url: "/WorkingHour/index",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#workingHourContainer").html(response);

            if (flag == 0) {
                $("#whfilter").empty()
            }
            $("#id").val(id);
            $("#doc_id").val(doc_id);
            $('#workingHourModal').modal('show');
          
        },
        error: function () {
        }
    });
}



function saveWorkinghour() {
    var id = $("#id").val();
    var doc_id = $("#doc_id").val();
    var wh_remarks = $("#wh_remarks").val();
    var wh_hours = $("#wh_hours").val();
    if (wh_hours > 0) {
        var data = new FormData();
        data.append('id', id);
        data.append('doc_id', doc_id);
        data.append('wh_remarks', wh_remarks);
        data.append('wh_hours', wh_hours);

        $.ajax({
            url: "/WorkingHour/Create",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                $("#workingHourContainer").html(response);
                $("#id").val(id);
                $("#doc_id").val(doc_id);
                $('#workingHourModal').modal('show');
                $("#remarks").val("");
                $("#wh_remarks").val("");
                $("#time").val("");
                $("#wh_hours").val("");
            },
            error: function () {
            }
        });
    }
    else {
        alert("Deduction Cannot be Negative!!!")
    }
}

function HrAppovalBySearch() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("rq_dep_id", $("#rq_dep_id").val());
    data.append("reportrange", $("#reportrange").val());
    data.append("rq_cre_by", $("#rq_cre_by").val());
    data.append("rq_cre_date", $("#rq_cre_date").val());
    $.ajax({
        url: "/OvertimeRequest/HrAppovalBySearch",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            $('#mytable').DataTable({
                dom: 'lBfrtip',
                buttons: [
                    'copyHtml5',
                    'excelHtml5',
                    'pdfHtml5'
                ]
            });
            $('#overlay').fadeOut()
        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}
function JQ_reject(id) {
    var reason = prompt("Reason For Rejection?? ", "");
   
    if (reason != null && reason != "") {
       
        var data = new FormData();
        data.append("id", id);
        data.append("reason", reason);

        $.ajax({
            url: "/OvertimeRequest/JQ_Reject",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.message == "Success") {
                   var $datatable = $('#mytable').DataTable();
		            $datatable.row($("#row" + id)).remove().draw();
                    
                }
                else {
                    alert(response.message);
                }
               
            },
            error: function () {
            }
        });
       
    }
    else {
        alert('please enter Reason!!!');
    }
}
function JQ_Approve(id) {
   
        var data = new FormData();
        data.append("id", id);
        $.ajax({
            url: "/OvertimeRequest/JQ_Approve",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.message == "Success") {
		        var $datatable = $('#mytable').DataTable();
		        $datatable.row($("#row" + id)).remove().draw();
		    //$('#myTable').dataTable().fnDeleteRow(row);
                   // $("#row" + id).remove();
                   // $('#mytable').DataTable().ajax.reload();
                }
                else {
                    alert(response.message);
                }

            },
            error: function () {
            }
        });

}
function userLoginHistory() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    data.append("id", $("#ll_cre_by").val());
    $.ajax({
        url: "/User/UserLoginHistoryBySearch",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            $('#mytable').DataTable({
                order:[],
                dom: 'lBfrtip',
                buttons: [
                    'copyHtml5',
                    'excelHtml5',
                    'pdfHtml5'
                ]
            });
            $('#overlay').fadeOut()
        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}

function SearcbTraingingData() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    data.append("id", $("#rq_cre_for").val());
    $.ajax({
        url: "/Training/TrainingData",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $('#overlay').fadeOut();
            $("#container").html(response);
            $('#mytable').DataTable({
                order: [],
                dom: 'lBfrtip',
                buttons: [
                    'copyHtml5',
                    'excelHtml5',
                    'pdfHtml5'
                ]
            });
        
        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}
function OpenModalForAddTraining() {
    $('#TrainingModal').modal('show');
}
function addTraining() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("tr_start_date", $("#from").val());
    data.append("tr_u_id", $("#tr_u_id").select2().val());
    $.ajax({
        url: "/Training/create",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "success") {
                $('#TrainingModal').modal('hide');
                $('#overlay').fadeOut()
            }
            else
            {
                alert(response.message);
                $('#overlay').fadeOut()
            }
        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}
function FinishTraing(id) {

    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("tr_id", id);
    data.append("tr_end_date", $("#tr_id" + id).val());
    $.ajax({
        url: "/Training/FinishTraing",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
               
                $("#tr_id" + id).prop("disabled", true);
                $("#btn" + id).prop("disabled", true);
                 $('#overlay').fadeOut()
            }
            else {
                alert(response.message);
                $('#overlay').fadeOut()
            }
        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}



function SearchUserShiftData() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    data.append("id", $("#us_u_id").val());
    $.ajax({
        url: "/UserShift/UserShiftData",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $('#overlay').fadeOut();
            $("#container").html(response);
            if (!$.fn.DataTable.isDataTable('#mytable')) {
                $('#mytable').DataTable({
                    order: [],
                    dom: 'lBfrtip',
                    buttons: [
                        'copyHtml5',
                        'excelHtml5',
                        'pdfHtml5'
                    ]
                });
            }

        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}
function OpenModalForAddUserShift() {
    $('#UserShiftModal').modal('show');
}
function addUserShilft() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("us_start_time", $("#us_start_time").val());
    data.append("us_end_time", $("#us_end_time").val());
    data.append("us_start_date", $("#us_start_date").val());
    data.append("us_u_id", $("#user").select2().val());
    if ($("#user").select2().val() != 0) {
        $.ajax({
            url: "/UserShift/Create",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.message == "Success") {
                    $('#UserShiftModal').modal('hide');
                    SearchUserShiftData();
                    $('#overlay').fadeOut();
                }
                else {
                    alert(response.message);
                    $('#overlay').fadeOut();
                }
            },
            error: function () {
                $('#overlay').fadeOut();
            }
        });
    } else {
        alert("Please select user !!!");
        $('#overlay').fadeOut();
    }
}
function FinishUserShift(id) {

    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("us_id", id);
    data.append("us_end_date", $("#us_id" + id).val());
    $.ajax({
        url: "/UserShift/FinishUserShift",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {

                $("#us_id" + id).prop("disabled", true);
                $("#btn" + id).prop("disabled", true);
                $('#overlay').fadeOut()
            }
            else {
                alert(response.message);
                $('#overlay').fadeOut()
            }
        },
        error: function () {
            $('#overlay').fadeOut()
        }
    });
}

function DeleteLeave(LeaveId) {
  
    var data = new FormData();
    data.append("LeaveId", LeaveId);
  
    $.ajax({
        url: "/Attendance/DeleteLeave",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            alert(response)
            location.reload();
        },
        error: function () {
            //$('#overlay').fadeOut()
        }
    });
}
function getUserReportingHeirarchy() {

    var data = new FormData();
    data.append("urh_u_id", $("#urh_u_id").select2().val());
    data.append("urh_reporting_to", $("#urh_reporting_to").select2().val());
    data.append("urh_priority", $("#urh_priority").val());

    $.ajax({
        url: "/User/getUserReportingHeirarchy",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#Container").html(response);
            loadDatatable("mytable");
        },
        error: function () {
        }
    });
}
function loadDatatable(tableid) {
    if (!$.fn.DataTable.isDataTable('#'+ tableid)) {
        $('#' + tableid).DataTable({
            order: [],
            dom: 'lBfrtip',
            buttons: [
                'copyHtml5',
                'excelHtml5',
                'pdfHtml5'
            ]
        });
    }
}
function AddUserReportingHeirarchy() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("urh_u_id", $("#urh_u_id").select2().val());
    data.append("urh_reporting_to", $("#urh_reporting_to").select2().val());
    data.append("urh_priority", $("#urh_priority").val());
    
    if ($("#urh_u_id").select2().val() != 0) {
        $.ajax({
            url: "/user/AddUserReportingHeirarchy",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.message == "Success") {
                    getUserReportingHeirarchy();
                    $('#overlay').fadeOut();
                }
                else {
                    alert(response.message);
                    $('#overlay').fadeOut();
                }
            },
            error: function () {
                $('#overlay').fadeOut();
            }
        });
    } else {
        alert("Please select user !!!");
        $('#overlay').fadeOut();
    }
}

function attandancedetails() {
    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    data.append("u_id", $("#User").select2().val());
    $.ajax({
        url: "/Attendance/AttendanceDetailsBySearch",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#Container").html(response);
            loadDatatable("mytable");
        },
        error: function () {
        }
    });
}