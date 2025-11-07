$(document).ready(function () {
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();
    var eventsdata = new Array;
    $("#DateFilter").hide();
    $("#rq_cre_by").select2({ width: '100%' });
    $("#rq_dep_id").select2({ width: '100%' });
    $("#rq_cre_for").select2({ width: '100%' });
    $("#l_leave_for").select2({ width: '100%' });
    $("#l_dep_id").select2({ width: '100%' });
    $("#l_designation").select2({ width: '100%' });
    $("#l_type").select2({ width: '100%' });
    $("#us_u_id").select2({ width: '100%' });
    $("#User").select2({ width: '100%' });
    $("#ud_user_id").select2({ width: '100%' });
    $("#user").select2({ width: '100%' });
    $("#tr_u_id").select2({ width: '100%' });
    $("#urh_u_id").select2({ width: '100%' });
    $("#urh_reporting_to").select2({ width: '100%' });
    $("#ud_depart_id").select2({ width: '100%' });
    $("#u_role_id").select2({ width: '100%' });
    $("#u_accomodation").select2({ width: '100%' });
    $("#machine").select2({ width: '100%' });
    $('select[name="Supervisors"]').select2({ width: '100%' });

    $("#SalaryRequiredDiv").hide();
    $("#SalaryRequestDetailDiv").hide();
   

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
                    $("#multiselect_to").empty();
                    $("#multiselect").empty();
                },
                error: function () {
                    $("#multiselect_to").empty();
                    $("#multiselect").empty();

                }
            });
        }
        // Open the first tab by default
        $(".tablinks:first").click();
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
        if ($("#multiselect_to option[value='" + $("#multiselect").val() + "']").length == 0) {
            $("#multiselect_to").append($("<option />").val($("#multiselect option:selected").val()).text($("#multiselect option:selected").text()));
            $("#multiselect option[value='" + $("#multiselect option:selected").val() + "']").remove();
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
    $('#forWhom').change(function () {
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
    if ($("#UserBioDepartment").length) {
        getUserBioDepartment();
    }
    if ($("#Leave").length) {
        getLeaveRequests();
    }
    if ($("#ReviewLeaveApplications").length) {
        getLeaveApplicationsForReview();
    }
    if ($("#LeaveApproval").length) {
        getLeaveApplicationsForHrApproval();
    }
    if ($("#ArchivedLeaves").length) {
        getArchivedLeaves();
    }
    if ($("#ManualPunching").length) {
        getMyTodaysPunchInfos();
    }
    if ($("#mytable").length) {
        $('#mytable').DataTable({
            dom: 'lBfrtip',
            buttons: [
                'copyHtml5',
                'excelHtml5',
                'pdfHtml5'
            ]
        });
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
            prev: "<",
            next: ">"
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

    function workflowDetailStatus(workflow, status) {
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
                var date2 = Date.parse($(this).find("#starttime" + value).val());
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

    function viewInsights(id, doc_id,flag) {
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

        $('button[name="btn_rejects"]').attr("disabled", true);

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
                    $('button[name="btn_rejects"]').removeAttr("disabled");
                },
                error: function () {
                    $('button[name="btn_rejects"]').removeAttr("disabled");
                }
            });

        }
        else {
            alert('please enter Reason!!!');
        }
    }
    function JQ_Approve(id) {

        $('button[name="btn_Approvals"]').attr("disabled", true);

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
                }
                else {
                    alert(response.message);
                }
              
                $('button[name="btn_Approvals"]').removeAttr("disabled");
            },
            error: function () {

                $('button[name="btn_Approvals"]').removeAttr("disabled");

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
        if ($("#us_day").val() != "") {
            data.append("us_start_time", $("#us_start_time").val());
            data.append("us_end_time", $("#us_end_time").val());
            data.append("us_break_from", $("#us_break_from").val());
            data.append("us_break_to", $("#us_break_to").val());
            data.append("us_start_date", $("#us_start_date").val());
            data.append("us_day", $("#us_day").val());
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
        } else {
            alert("Please Select Day");
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

        if ($("#urh_u_id").select2().val() != 0 && $("#urh_reporting_to").select2().val() != 0 && $("#urh_priority").val() != 0) {
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
            alert("Please Enter all data !!!");
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
    function getEmployeeInformation() {
        var data = new FormData();
        data.append("Type", $("#Type").val());
        data.append("u_id", $("#User").select2().val());
        $.ajax({
            url: "/User/getEmployeeInformation",
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
    function OpenCancelationModal(id) {

        $('#CancelationModel').modal('show');
        $("#id").val(id);

    }
    function CancelEmployee() {
        var data = new FormData();
        if ($("#u_cancelation_date").val() != "" && $("#id").val() != 0) {
            data.append("u_cancelation_date", $("#u_cancelation_date").val());
            data.append("u_id", $("#id").val());
            $.ajax({
                url: "/User/CancelEmployee",
                type: "POST",
                contentType: false,
                processData: false,
                cache: false,
                data: data,
                success: function (response) {
                    if (response.message == "Success") {
                        $('#CancelationModel').modal('hide');
                        $("#labl_"+$("#id").val()).text($("#u_cancelation_date").val());
                        $("#btn_"+$("#id").val()).remove();
                        $('#overlay').fadeOut();
                    }
                    else {
                        alert(response.message);
                        $('#overlay').fadeOut();
                    }
                },
                error: function () {
                }
            });
        } else {
            alert("Please enter all Data !!!");
        }
    }
function deleteEmployeeReportingHirarchy(urh_id) {
    var data = new FormData();
    data.append("urh_id", urh_id);
    $.ajax({
        url: "/User/deleteEmployeeReportingHirarchy",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
                var $datatable = $('#mytable').DataTable();
                $datatable.row($("#row" + urh_id)).remove().draw();
            }
            else {
                alert(response.message);
            }
        },
        error: function () {
        }
    });

}

function AddFoodDetails() {

    var data = new FormData();
    var isValid = true;
    var breakFeedback = $("#lbl_BreakfastFeedback").text();
    var lunchFeedback = $("#lbl_LunchFeedback").text();
    var supperFeedback = $("#lbl_SupperFeedback").text();
    var cmdSuggestion = $("#comments").val();
    //$('#lbl_BreakfastFeedback,#lbl_LunchFeedback,#lbl_SupperFeedback').each(function () {
    //    var  checkmeals= $.trim($(this).is(':contains("choose")'));
    //    if ($.trim($(this).text()) == '') {
    //        isValid = false;
    //        $(this).css({ "border": "3px solid red", "background": "#FF0000", "text-decoration":"thickness"});
    //        $(this).text("choose the " + $(this).attr("name") + " feedback رائے کا انتخاب کریں؟");
    //    }
    //    else if (checkmeals != 'false') {
    //        isValid = false;
    //        $(this).text("choose the " + $(this).attr("name") + " feedback رائے کا انتخاب کریں؟");
    //    }
    //    else {
    //        $(this).css({ "border": "", "background": "" });
    //    }
    //});
    //if (isValid == false)
    //    return false;
    data.append("F_Breakfeedback", breakFeedback);
    data.append("F_Lunchfeedback", lunchFeedback);
    data.append("F_Supperfeedback", supperFeedback);
    data.append("F_cmdSuggestion", cmdSuggestion);
    $.ajax({
        url: "/Food/AddFoodDetails",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
           
            if (response.message == "Success") {
                alert(response.message);
                window.location.replace("/Home/index");
               
            }
            else {
                alert(response.message);
              
            }
           

        },
        error: function (xhr, status, error) {
            var errorMessage = xhr.status + ': ' + xhr.statusText
            alert('Error - ' + errorMessage);
        }
    });
}

function AssignFeedback(Firstid, Secondid) {
    if (Firstid == 'b') { $('#lbl_BreakfastFeedback').text(Secondid); $("#lbl_BreakfastFeedback").css({ "border": "", "background": "" }); }
    if (Firstid == 'l') { $('#lbl_LunchFeedback').text(Secondid); $("#lbl_LunchFeedback").css({ "border": "", "background": "" }); }
    if (Firstid == 's') { $('#lbl_SupperFeedback').text(Secondid); $("#lbl_SupperFeedback").css({ "border": "", "background": "" }); }
    Firstid = "";
    Secondid = "";
}

function emoji(clickid) {
    var stars = document.getElementsByClassName("fas");
    var emoji = document.getElementById("emoji");
    var Firstid = clickid.split('_')[0].trim();
    var Secondid = clickid.split('_')[1].trim();
    var jj = '#' + Firstid + '_' + 'poor';
    if (clickid == "b_poor" || clickid == "l_poor" || clickid == "s_poor") {
        $('#' + Firstid+'_'+'poor').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'bad').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'okay').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'good').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'excellent').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'emoji').css("transform", `translateX(${0}px)`);
        AssignFeedback(Firstid, Secondid);
    }
    if (clickid == "b_bad" || clickid == "l_bad" || clickid == "s_bad") {
        $('#' + Firstid + '_' + 'poor').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'bad').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'okay').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'good').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'excellent').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'emoji').css("transform", `translateX(${-100}px)`);
        AssignFeedback(Firstid, Secondid);
    }
    if (clickid == "b_okay" || clickid == "l_okay" || clickid == "s_okay") {
        $('#' + Firstid + '_' + 'poor').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'bad').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'okay').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'good').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'excellent').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'emoji').css("transform", `translateX(${-200}px)`);
        AssignFeedback(Firstid, Secondid);
    }
    if (clickid == "b_good" || clickid == "l_good" || clickid == "s_good") {
        $('#' + Firstid + '_' + 'poor').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'bad').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'okay').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'good').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'excellent').css("color", "#e4e4e4");
        $('#' + Firstid + '_' + 'emoji').css("transform", `translateX(${-300}px)`);
        AssignFeedback(Firstid, Secondid);
    }

    if (clickid == "b_excellent" || clickid == "l_excellent" || clickid == "s_excellent") {
        $('#' + Firstid + '_' + 'poor').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'bad').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'okay').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'good').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'excellent').css("color", "#ffd93b");
        $('#' + Firstid + '_' + 'emoji').css("transform", `translateX(${-400}px)`);
        AssignFeedback(Firstid, Secondid);
    }

}

function GetFoodFeedBackDaily() {
    //var test = $('#c_date1').val();
    //if ($('#c_date1').val() == "") {
    //    alert("choose date..! ");
    //    return false;
    //}
    var tst = $('#reportrange')
    var data = new FormData();
    //data.append("date", $("#c_date1").val());
    data.append("date", $("#reportrange").val());
    $.ajax({
        url: "/Food/GetFoodFeedBackReportByDate",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}
function SkipFoodFeedback() {
    window.location.replace("/Home/index");
}


function getUserBioDepartment() {

    var data = new FormData();
    $.ajax({
        url: "/UserBioDepartment/getUserBioDepartment",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}

function getInAndOutLogBySearch() {

    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    $.ajax({
        url: "/InAndOut/getInAndOutLogBySearch",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}
function getAccomodations() {
  
    var data = new FormData();
    data.append("name", $("#name").val());
    $.ajax({
        url: "/Accomadation/getAccomodations",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
           
            $("#container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}

function OpenModalForAddAccommodation() {
    $("#ac_active_yn").val("Y")
    $("#ac_active_yn").hide();
    $("#lbl_ac_active_yn").hide();
    $('#AccomodationModal').modal('show');

}
function OpenModalForEditAccommodation(ac_id, ac_name, ac_first_punch_type, ac_active_yn) {
    $("#ac_name").val(ac_name);
    $("#ac_id").val(ac_id);
    $("#ac_first_punch_type").val(ac_first_punch_type);
    $("#ac_active_yn").val(ac_active_yn);
    $("#ac_active_yn").show();
    $("#lbl_ac_active_yn").show();
    $('#AccomodationModal').modal('show');
}

function AddOrUpdateAccomodation() {
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("ac_name", $("#ac_name").val());
    data.append("ac_id", $("#ac_id").val());
    data.append("ac_first_punch_type", $("#ac_first_punch_type").val());
    data.append("ac_active_yn", $("#ac_active_yn").val());
    $.ajax({
        url: "/Accomadation/AddOrUpdateAccomodation",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
               
                $("#ac_name").val("");
                $("#ac_id").val(0);
                $("#ac_first_punch_type").val('Y');
                getAccomodations();
                $('#overlay').fadeOut()
                $('#AccomodationModal').modal('hide');
               
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
function DeleteAccomodation(id) {
    var data = new FormData();
    data.append("id", id);
    $.ajax({
        url: "/Accomadation/DeleteAccomodation",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
                 getAccomodations();
            }
            else {
                alert(response.message);
                $('#overlay').fadeOut()
            }
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });
}

function getInAndOutReport() {

    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    data.append("u_id", $("#user").select2().val());
    data.append("ac_id", $("#ac_id").select2().val());
    $.ajax({
        url: "/InAndOut/getInAndOutReport",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}



function AddRegularizationPunch() {
    $("#InAndOutModal").modal("show");

    $("#io_u_id").val($("#user").select2().val());
    $("#io_u_id").select2({ width: '100%' });

}


function AddInAndOut() {

    var data = new FormData();

    var inandout = $("input[name='InAndOut']:checked").val();

    data.append("io_u_id", $("#io_u_id").select2().val());
    data.append("io_punchtime", $("#io_punchtime").val());
    data.append("io_punch_type", inandout);
    data.append("io_remarks", $("#io_remarks").val());
    $.ajax({
        url: "/InAndOut/AddInAndOut",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
                getInAndOutReport();
                $("#InAndOutModal").modal("hide");
            }
            else {
                alert(response.message);
                $('#overlay').fadeOut()
            }
            $("#io_u_id").val(0);
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}


function changeInAndOut(id) {
    var data = new FormData();
    var punchtype = $("input[name='InAndOut"+id+"']:checked").val();

    data.append("io_id", id);
    data.append("io_punch_type", punchtype);
 
    $.ajax({
        url: "/InAndOut/updateInAndOutPunchType",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
                getInAndOutReport();
                alert("Successfully Updated !!!");
            }
            else {
                alert(response.message);
                $('#overlay').fadeOut()
            }
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });
}


function getAccomodationWiseInAndOut() {

    var data = new FormData();
    var inandout = $("input[name='InAndOut']:checked").val();
    data.append("ac_id", $("#ac_id").select2().val());
    data.append("status", inandout);
    $.ajax({
        url: "/InAndOut/getAccomodationWiseInAndOut",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}

function changeInAndOutUserWise(u_id) {
    var data = new FormData();
    var punchtype = $("input[name='InAndOut" + u_id + "']:checked").val();

    data.append("io_u_id", u_id);
    data.append("io_punch_type", punchtype);

    $.ajax({
        url: "/InAndOut/updateInAndOutPunchTypeUserWise",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
                getAccomodationWiseInAndOut();
                alert("Successfully Updated !!!");
            }
            else {
                alert(response.message);
                $('#overlay').fadeOut()
            }
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });
}

function getLeaveRequests() {

    var data = new FormData();
    $.ajax({
        url: "/Leave/getLeaveRequests",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#Container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}

function OpenModalForCreateLeave() {
   
    getRemainingLeaveOnly();
    if ($('#l_leave_from').val()) {
        updateDayOfWeek('#l_leave_from', 'dayOfWeekFrom');
    }
    if ($('#l_leave_to').val()) {
        updateDayOfWeek('#l_leave_to', 'dayOfWeekTo');
    }
    enableFormControls('#createLeaveForm');
    $("#l_leave_days").prop("disabled", true);
    clearLeaveRequestForm();

    $("#LeaveModal").modal("show");
    $("#upd_btn").show();

}

function createOrUpdateLeave() {

    var l_leave_for = $("#l_leave_for").val();
    var l_dep_id = $("#l_dep_id").val();
    var l_type = $("#l_type").val();
    var l_reason = $("#l_reason").val();
    var l_leave_for = $("#l_leave_for").val();
    var l_leave_from = $("#l_leave_from").val();
    var l_leave_to = $("#l_leave_to").val();
    var fromDate = new Date(l_leave_from);
    var toDate = new Date(l_leave_to);
    var l_leave_days = $("#l_leave_days").val();
    var l_salary_month = $("#l_salary_month").val();
    var l_required_amount = $("#l_required_amount").val();
    var l_required_date = $("#l_required_date").val();
    var salary_required = $('input[name="l_salary_required"]:checked').val();
    var files = $('#files')[0].files;
    var AttachementRequired = $("#AttachmentRequired").val();

    if (l_leave_for != 0 && l_leave_days > 0 && l_dep_id != 0 && l_type != 0 && 
        ((salary_required == 'Y' && l_salary_month != '' && l_required_amount > 0 && l_required_date != "") || (salary_required == 'N'))
        && ((files.length > 0 && AttachementRequired == "Y") || (AttachementRequired=="N"))
    ) {
        if (toDate < fromDate) {
            alert('The "Leave To" date must be greater than the "Leave From" date.');
        } else {

            var data = new FormData($('#createLeaveForm')[0]);
            $.ajax({
                url: "/Leave/createOrUpdateLeave",
                type: "POST",
                contentType: false,
                processData: false,
                cache: false,
                data: data,
                success: function (response) {
                    if (response.message == "Success") {
                        getLeaveRequests();
                        alert("Successfully Created");
                        clearLeaveRequestForm();
                        $("#LeaveModal").modal("hide");
                    }
                    else {

                        alert(response.message);
                        $('#overlay').fadeOut()
                    }
                },
                error: function () {
                    $('#overlay').fadeOut();
                }
            });
        }
    }
    else {
        alert("Please Enter all the Data !!");
    }
}
function clearLeaveRequestForm() {
    $("l_id").val("0");
    $("#l_dep_id").select2().val("0").trigger("change");
    $("#l_designation").select2().val("0").trigger("change");
    $("#l_type").select2().val("0").trigger("change");
    $("#l_reason").val("");
    $("#l_salary_month").val("");
    $("#l_required_amount").val("");
    $("#l_required_date").val("");
    $("#l_contact_no1").val("");
    $("#l_contact_no2").val("");
    $("#l_address").val("");
    $("#l_leave_days").val("");
    $("#l_leave_days").prop("disabled", true);
    $('input[name="l_salary_required"][value="N"]').prop('checked', true);
    $('#l_leave_from').val("");
    updateDayOfWeek($('#l_leave_from'), 'dayOfWeekFrom');
    $('#l_leave_to').val("");
    updateDayOfWeek($('#l_leave_to'), 'dayOfWeekTo');
    $("#SalaryRequiredDiv").hide();
    $("#SalaryRequestDetailDiv").hide();
}


function editLeaveApplication(l_id,flag)
{
    $("#upd_btn").show();
    var data = new FormData();
    data.append("l_id", l_id);
    $.ajax({
        url: "/Leave/getLeave",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            setFormValues('#createLeaveForm', response);
           
            if (flag == "Edit") {

                enableFormControls('#createLeaveForm');
                $("#l_leave_days").prop("disabled", true);
                $("#attdiv").show();
                $("#upd_btn").show();
               
            }
            else
            {
                disableFormControls('#createLeaveForm');
                $("#l_leave_days").prop("disabled", true);
                $("#attdiv").hide();
                $("#upd_btn").hide();
            }
            $("#LeaveModal").modal("show");
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });
}

function getLeaveApplicationsForReview() {
    var data = new FormData();
    $.ajax({
        url: "/Leave/getLeaveApplicationsForReview",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#Container").html(response);
            $('select[name="Supervisors"]').select2({ width: '100%' });
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });
}

function OpenReviewLeaveRequestModal(l_id) {
    var data = new FormData();
    data.append("l_id", l_id);
    $.ajax({
        url: "/Leave/getViewLeaveDetail",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#LeaveDetailContainer").html(response);
            getRemainingLeaveOnly();
            findNoOfDaysBetweenTwoDates();
            updateDayOfWeek($('#l_leave_from'), 'dayOfWeekFrom');
            updateDayOfWeek($('#l_leave_to'), 'dayOfWeekTo');
            $('input[name="l_salary_required"]').trigger("change");
            $("#LeaveModal").modal("show");
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });
   
}


function AuthorizeLeave() {

    var l_id = $("#l_id").val();
    var l_authorization = $("#l_authorization").val();
    if (l_id != 0 && l_authorization != "0") {
        var data = new FormData();
        data.append("l_id", l_id);
        data.append("l_authorization", l_authorization);
        $.ajax({
            url: "/Leave/AuthorizeLeave",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.message == "Success") {
                    getLeaveApplicationsForReview();
                    $("#LeaveModal").modal("hide");

                }
                else {
                    alert(response.message);
                    $('#overlay').fadeOut()
                }
            },
            error: function () {
                $('#overlay').fadeOut();
            }
        });
    }
    else {
        alert("Please Select Authorization !!");
    }
}




function getLeaveApplicationsForHrApproval() {

    var data = new FormData();
    $.ajax({
        url: "/Leave/getLeaveApplicationsForHrApproval",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#Container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}



function ApproveLeave(id, type) {
  
    var data = new FormData();
    data.append("id", id);
    data.append("type", type);
    $.ajax({
        url: "/Leave/ApproveLeave",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
                getLeaveApplicationsForHrApproval();
                alert("Successfully Approved");
            }
            else {
                alert(response.message);
                $('#overlay').fadeOut()
            }
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });
    
}


function deleteLeaveApplication(l_id) {
    var data = new FormData();
    data.append("l_id", l_id);
    $.ajax({
        url: "/Leave/deleteLeaveApplication",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
                var datatable = $("#mytable").DataTable();
                datatable.row("#tr" + l_id).remove().draw();
                alert("Successfully Deleted");
            }
            else {
                alert(response.message);
                $('#overlay').fadeOut()
            }
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });
}

function getLeaveReport() {

    var data = new FormData();
    $.ajax({
        url: "/Leave/getLeaveReport",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#Container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}



function getLeaveReport() {

    var data = new FormData();
    var u_id = $("#User").val();
    var daterange = $("#reportrange").val();
    var Type = $("#Type").val();
    var fullhistory = $('input[name="fullhistory"]').prop("checked");
    if (daterange != "") {
        data.append("u_id", u_id);
        data.append("reportrange", daterange);
        data.append("type", Type);
        data.append("fullhistory", fullhistory);

        $.ajax({
            url: "/Leave/getLeaveReport",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                $("#Container").html(response);
                loadDatatable('mytable');
            },
            error: function () {
            }
        });
    } else {
        alert("Please enter Date Range");
    }

}

function HideDateRange() {
    $("#DateFilter").toggle();
    if ($("#Verified").length) {
       
    }
    if ($("#WarehouseVerified").length) {
        
    }
}


function getRemainingLeave() {

    var u_id = $("#User").val();

    var data = new FormData();
    data.append("u_id", u_id);
    $.ajax({
        url: "/Leave/getRemainingLeave",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#Container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });
}

function loadAttachment(doc_id, fun_doc_id) {
 
    var data = new FormData();
    data.append('fun_doc_id', fun_doc_id);
    data.append('doc_id', doc_id);
    $.ajax({
        url: "/Attachment/GetAttachmentsByDocument",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#attachmentContainer").html(response);
            $(".tablinks:first").click();
            $("#AttachmentModal").modal("show");
          
        },
        error: function () {
            $("#attachmentContainer").html("Error loading attachments.");
        }
    });
}

$(document).on('click', '.tablinks', function () {
    var tabName = $(this).data("tab");

    // Hide all tab content
    $(".tabcontent").hide();

    // Remove active class from all buttons
    $(".tablinks").removeClass("active");

    // Show the current tab and add active class to the clicked button
    $("#" + tabName).show();
    $(this).addClass("active");
});


function getArchivedLeaves() {

    var data = new FormData();
    $.ajax({
        url: "/Leave/getArchivedLeaves",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            $("#Container").html(response);
            loadDatatable('mytable');
        },
        error: function () {
            $('#overlay').fadeOut();
        }
    });

}

function OpenArchivedLeaveModal() {

    $("#ArchivedLeaveModal").modal("show");
}

function createArchivedLeave() {
    var u_id = $("#User").val();
    var leave_days = $("#leaveDays").val();

    if (u_id != 0 && leave_days != 0) {
        var data = new FormData();
        data.append("u_id", u_id);
        data.append("leave_days", leave_days);
        $.ajax({
            url: "/Leave/createArchivedLeave",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.message == "Success") {
                    alert("Successfully Created");
                    $("#ArchivedLeaveModal").modal("hide");
                    $("#User").select2().val("").trigger("change");
                    $("#leaveDays").val("");
                    getArchivedLeaves()
                    $('#overlay').fadeOut()
                }
                else {
                    alert(response.message);
                    $('#overlay').fadeOut()
                }
            },
            error: function () {
                $('#overlay').fadeOut();
            }
        });
    }
    else {
        alert("Please Enter All Data");
    }
}

function updateArchivedLeave(al_id) {


    var leave_days = $("#inp"+al_id).val();

    if (al_id != 0 && leave_days != 0) {
        var data = new FormData();
        data.append("al_id", al_id);
        data.append("leave_days", leave_days);
        $.ajax({
            url: "/Leave/updateArchivedLeave",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.message == "Success") {
                    alert("Successfully Updated");
                }
                else {
                    alert(response.message);
                    $('#overlay').fadeOut()
                }
            },
            error: function () {
                $('#overlay').fadeOut();
            }
        });
    }
    else {
        alert("Please Enter All Data");
    }
}


function findNoOfDaysBetweenTwoDates() {
   
    const leaveFrom = $('#l_leave_from').val();
    const leaveTo = $('#l_leave_to').val();

   
    if (leaveFrom && leaveTo) {
      
        const dateFrom = new Date(leaveFrom);
        const dateTo = new Date(leaveTo);
        const timeDifference = dateTo.getTime() - dateFrom.getTime();
        const dayDifference = timeDifference / (1000 * 3600 * 24)+1;

        $("#l_leave_days").val(dayDifference);
    }
}

function getRemainingLeaveOnly() {
    var u_id = $("#l_leave_for").val();
    if (u_id != 0) {
        var data = new FormData();
        data.append("u_id", u_id);
        $.ajax({
            url: "/Leave/getRemainingLeaveOnly",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                $("#remainingContainer").html(response);
            },
            error: function () {
                $('#overlay').fadeOut();
            }
        });
    }
}

function updateDayOfWeek(dateInput, dayOfWeekId) {
    findNoOfDaysBetweenTwoDates();
    var dateValue = $(dateInput).val();
    if (dateValue) {
        var date = new Date(dateValue);
        var options = { weekday: 'long' };
        var dayOfWeek = new Intl.DateTimeFormat('en-US', options).format(date);
        $('#' + dayOfWeekId).attr("class", "btn btn-default");
        $('#' + dayOfWeekId).text(dayOfWeek);
    } else {
        $('#' + dayOfWeekId).attr("class", "");
        $('#' + dayOfWeekId).text('');
    }
}

function setMinDateForInputs(flag) {
    if (flag == true) {
        var today = new Date().toISOString().split('T')[0];
        $('#l_leave_from').attr('min', today);
        $('#l_leave_to').attr('min', today);
        $('#l_leave_from').val("");
        updateDayOfWeek($('#l_leave_from'), 'dayOfWeekFrom');
        $('#l_leave_to').val("");
        updateDayOfWeek($('#l_leave_to'), 'dayOfWeekTo');
    }
    else {
        $('#l_leave_from').removeAttr('min');
        $('#l_leave_to').removeAttr('min');
    }
}



$('#l_leave_from').change(function () {
    updateDayOfWeek(this, 'dayOfWeekFrom');
    SalaryRequiredChangeTrigger();
});

function SalaryRequiredChangeTrigger() {
    var date = new Date($('#l_leave_from').val());
    var day = ("0" + date.getDate()).slice(-2);
    var l_type = $('#l_type').val();
    var fromdate = "";
    if (!isNaN(date.getTime())) {
        fromdate = date.toISOString().split('T')[0];
        $('#l_leave_to').attr('min', fromdate);
    }
    if (day >= 20 && (l_type == 1 || l_type == 4)) {

        if (!isNaN(date.getTime())) {
            $('#l_salary_month').attr('min', fromdate);
            $('#l_required_date').attr('min', fromdate);
        }
        $("#SalaryRequiredDiv").show();

    } else {
        $("#SalaryRequiredDiv").hide();
        $('#l_salary_month').val("");
        $('#l_required_amount').val("");
        $('#l_required_date').val("");
        $("#SalaryRequestDetailDiv").hide();
    }

 
}


$('#l_leave_to').change(function () {
    updateDayOfWeek(this, 'dayOfWeekTo');
   
});



function onchangeLeaveType() {
    var l_type = $('#l_type').val();
    if (l_type==1) {
        setMinDateForInputs(true);
    }
    else {
        setMinDateForInputs(false);
    }

    SalaryRequiredChangeTrigger();

    if (l_type != 0) {
        var data = new FormData();
        data.append("id", l_type);
        $.ajax({
            url: "/Leave/onchangeLeaveType",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                $("#AttachmentRequired").val(response.attachmentRequired);
            },
            error: function () {
                $('#overlay').fadeOut();
            }
        });
    }
}


$('input[name="l_salary_required"]').change(function () {
    $('input[name="l_salary_required"]').not(this).prop('checked', false); // Uncheck other checkboxes
    var selectedValue = $(this).val();
    if (selectedValue == "Y") {
        $("#SalaryRequestDetailDiv").show();
    }
    else {
        $("#SalaryRequestDetailDiv").hide();
    }
});


function setFormValues(formSelector, data) {
    $.each(data, function (key, value) {
        var $field = $(formSelector + ' [name=' + key + ']');
        
        if ($field.length === 0) {
            // Skip if no element is found for the name
            return true;
        }

        if (value === null || value === undefined) {
            // Skip if the value is null or undefined
            return true; // equivalent to 'continue' in jQuery's each loop
        }

        if ($field.is(':checkbox')) {
            $('input[name=' + key + '][value="' + value + '"]').prop('checked', true).trigger("change");
        } else if ($field.is(':radio')) {
            $field.filter('[value="' + value + '"]').prop('checked', true).trigger("change");
        } else if ($field.hasClass('select2-hidden-accessible')) { // Check if it's a Select2 element
            $field.val(value).trigger('change');
        } else if ($field.is('input[type="date"]')) {
            // Check if it's a date input and format the date value correctly
            var dateValue = value.split("T")[0]; // Extract the date part
            $field.val(dateValue).trigger("change");
        } else if ($field.is('input[type="month"]')) {
            // Check if it's a month input and ensure the value is in the correct format
            var monthValue = value.slice(0, 7); // Extract the YYYY-MM part
            $field.val(monthValue).trigger("change");
        } else {
            $field.val(value).trigger("change");
        }
    });
}


function disableFormControls(formSelector) {
    $(formSelector).find('input, select, textarea, button').prop('disabled', true);
}
function enableFormControls(formSelector) {
    $(formSelector).find('input, select, textarea, button').removeAttr("disabled");

}


function HrLeaveApprove(type) {
    var l_id = $("#l_id").val();
    var ticket = $('input[name="l_ticket"]:checked').val();
    var l_type = $('#l_type').val();
    if ((ticket != "" && l_type == 1) || (l_type!=1)) {
        var data = new FormData();
        data.append("id", l_id);
        data.append("type", type);
        data.append("ticket", ticket);
        $.ajax({
            url: "/Leave/ApproveLeave",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.message == "Success") {
                    alert("Successfully Approved");
                    getLeaveApplicationsForHrApproval();
                    $("#LeaveModal").modal("hide");
                }
                else {
                    alert(response.message);
                    $('#overlay').fadeOut()
                }
            },
            error: function () {
                $('#overlay').fadeOut();
            }
        });
      
    } else {

        alert("Select Ticket !!!");
    }
}


function SendToAnotherSupervisor(l_id) {

    var supervisor=$("#su" + l_id).val();

    var data = new FormData();
    data.append("l_id", l_id);
    data.append("supervisor", supervisor);
    if (supervisor != 0) {
        $.ajax({
            url: "/Leave/SendToAnotherSupervisor",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.message == "Success") {
                    getLeaveApplicationsForReview();
                }
                else {
                    alert(response.message);
                }
            },
            error: function () {
                $('#overlay').fadeOut();
            }
        });
    }
    else {
        alert("Please Select Supervisor !!");
    }
}


function getOvertimeFullDetails() {
    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    data.append("type", $("#type").val());
    data.append("user", $("#user").val());
   
    $.ajax({
        url: "/OvertimeRequest/getOvertimeFullDetails",
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

function getApprovedOtDetails() {
    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    data.append("type", $("#type").val());
    data.append("user", $("#user").val());

    $.ajax({
        url: "/OvertimeRequest/getApprovedOtDetails",
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

function addManualPunching() {
   
    $('#overlay').fadeIn();
    var data = new FormData();
    data.append("machine", $("#machine").select2().val());
    $.ajax({
        url: "/Attendance/addManualPunching",
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: data,
        success: function (response) {
            if (response.message == "Success") {
                getMyTodaysPunchInfos();
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

function getMyTodaysPunchInfos() {
    var data = new FormData();
    $.ajax({
        url: "/Attendance/getMyTodaysPunchInfos",
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


function getOvertimeReportNew() {
    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    data.append("type", $("#type").val());
    data.append("user", $("#user").val());

    $.ajax({
        url: "/OvertimeRequest/getOvertimeReportNew",
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

function getAttendanceReport() {
    var data = new FormData();
    data.append("reportrange", $("#reportrange").val());
    data.append("user", $("#user").select2().val());

    $.ajax({
        url: "/Attendance/getAttendanceReport",
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