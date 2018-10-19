function getDefaultRemoteDatatableSettings($datatable) {
    /*-------------------------------
        getDefaultRemoteDatatableSettings (Alex)
    ---------------------------------
    
        parameters:
            datatable
                The jQuery element of the table
        summary
            Gets the default remote settings
    
    */
    var $columnHeaders = $datatable.find('thead tr.datatable-column-headers th');
    // var dataProperties = _.map($columnHeaders, getDatatableHeaderColumnData);
    var dataTableOlang = {
        "aria": {
            "sortAscending": ": activate to sort column ascending",
            "sortDescending": ": activate to sort column descending"
        },
        "emptyTable": "No data available in table",
        "info": "Showing _START_ to _END_ of _TOTAL_ entries",
        "infoEmpty": "No entries found",
        "infoFiltered": "(filtered from _MAX_ total entries)",
        "lengthMenu": "_MENU_ entries",
        "search": "Search:",
        "zeroRecords": "No Records Found."
    };
    return {
        "columns": [], //dataProperties,
        "bAutoWidth": false,
        "bProcessing": true,
        "bStateSave": false,
        pagingType: "full_numbers",
        "bServerSide": true,
        "oDataUrl": null,
        "oDataViaJsonp": false,	// set to true for cross-domain requests
        "oDataAbort": false,
        "ajax": null,
        "language": dataTableOlang,
        "fnDrawCallback": function () { },
        "fnInitComplete": function () {
            $datatable.closest('div.dataTables_wrapper').find('div.datatableLengthFilter').find('.dataTables_filter').find('input').unbind();
            $datatable.closest('div.dataTables_wrapper').find('div.datatableLengthFilter').find('.dataTables_filter').find('input')
                .bind('keyup keydown',
                    function (e) {
                        switch (e.type) {
                            case 'keyup':
                                if (e.keyCode === 13) {
                                    $datatable.fnFilter(this.value);
                                }
                                else if (e.keyCode === 46 || e.keyCode === 8) {
                                    if ($(this).val() === '') $datatable.fnFilter(this.value);
                                }
                                break;
                            case 'keydown':
                                if (e.keyCode === 13)
                                    e.preventDefault();
                                break;
                        }
                    });
        },
        "fnServerData": null,// function (sSource, aoData, fnCallback, oSettings) {
        //if (oSettings.oDataUrl == undefined
        //|| oSettings.oDataUrl == null || oSettings.oDataUrl == '') {
        //    oSettings.jqXHR = api.customeDTAjax(sSource, aoData, fnCallback);
        //} //For Set Hearder.
        // },
        "fnServerParams": null,// function (aoData) {
        //  aoData.push({ name: "sDatatableId", value: $datatable.attr('id') });
        // },
        "sAjaxSource": null //$datatable.attr('data-url')
    };
}

function InitDataTable(settings) {
    var defaultParameters = {
        tableId: "",
        isServerSide: true,
        isCustomeAjax: false,
        extendedSettings: null
    }
    $.extend(defaultParameters, settings);

    var tableId = defaultParameters.tableId,
        isServerSide = defaultParameters.isServerSide,
        isCustomeAjax = defaultParameters.isCustomeAjax,
        extendedSettings = defaultParameters.extendedSettings;

    var $table = $('#' + tableId);
    var ths = $('#' + tableId).find('thead tr:last th').map(function () {
        return this.innerHTML;
    }).get();

    var addTh = false;
    $.each(ths, function (key, value) {
        if (value.indexOf('<br>') > -1 || value.indexOf('<br/>') > -1 || value.indexOf('<br />') > -1) {
            addTh = true;
            return false;
        }
        return true;
    });
    if (addTh)
        $('#' + tableId).find('thead tr:last').after('<tr>' + $('#' + tableId).find('thead tr:last').html() + '</tr>');
    //var settings = isServerSide === true ? getDefaultRemoteDatatableSettings($table) : getDefaultLocalDatatableSettings($table);
    var settings = getDefaultRemoteDatatableSettings($table);
    if (extendedSettings !== undefined) {
        var baseInitComplete = settings["fnInitComplete"];
        var extendedInitComplete = extendedSettings["fnInitComplete"];
        extendedSettings["fnInitComplete"] = function () {
            if (extendedInitComplete !== undefined)
                extendedInitComplete();
            baseInitComplete();
        };
        //Set Odata Ajax Call Back.
        if (extendedSettings.oDataUrl !== undefined
            && extendedSettings.oDataUrl != null && extendedSettings.oDataUrl != '') {
            extendedSettings.ajax = ajaxOData;
        }

        settings = $.extend({}, settings, extendedSettings);
    }
    //For Custome Ajax Call.
    if (isCustomeAjax) {
        $table.DataTable(settings);
    } else {
        delete settings.fnServerData;
        delete settings.fnServerParams;
        delete settings.sAjaxSource;
        $table.DataTable(settings);
    }


    $(document).on('focus', '.datatable-column-filters input', function () {
        $(this).attr('autocomplete', 'off');
    });

    $(document).on('keydown keyup', '.datatable-column-filters input', function (e) {
        switch (e.type) {
            case 'keyup':
                if (e.keyCode === 13) {
                    e.preventDefault();
                    search(this);
                    return false;
                }
                else if (e.keyCode === 46 || e.keyCode === 8) {
                    if ($(this).val() === '') search(this);
                }
                break;
            case 'keydown':
                if (e.keyCode === 13)
                    e.preventDefault();
                break;

        }
    });

    function search(element) {
        var datatable = $('#' + $(element).closest('div.dataTables_wrapper').attr('id').replace('_wrapper', '')).dataTable();
        var jqXHR = datatable.fnSettings().jqXHR;

        //if (jqXHR != null) {
        //    jqXHR.abort();
        //}

        if (jqXHR === null) {
            jqXHR.abort();
        }

        datatable.fnFilter($(element).val(), parseInt($(element).closest('div.controls').attr('data-column')));
    }

    //Used to set custom style for show record length and search filter
    // $($('.dataTables_wrapper')[0].childNodes[0]).addClass('datatableLengthFilter');
}

function getDatatableHeaderColumnData(datatableHeader) {
    // each column has specific options. These are written to data-... attributes on the <th> tag
    // this method reads those data-... properties and converts them into an mDataProp object that can be used with the datatables API.
    var $datatableHeader = $(datatableHeader);
    var property = $datatableHeader.data('property');
    var sSearchable = $datatableHeader.data('searchable');
    var bSortable = $datatableHeader.data('sortable');
    var sWidth = $datatableHeader.data('width');
    var bVisible = $datatableHeader.data('visible');
    var sClass = $datatableHeader.data('class');
    var sDefaultContent = $datatableHeader.data('default-content');

    var columnData = {
        mDataProp: property,
        name: property
    };

    if (isDefined(sWidth) && sWidth != null && sWidth.length > 0)
        columnData.sWidth = sWidth;

    if (isDefined(sSearchable) && sSearchable != null)
        columnData.bSearchable = sSearchable;

    if (isDefined(bSortable) && bSortable != null)
        columnData.bSortable = bSortable;

    if (isDefined(bVisible) && bVisible != null)
        columnData.bVisible = bVisible;

    if (isDefined(sClass) && sClass != null && sClass.length > 0)
        columnData.sClass = sClass;

    if (isDefined(sDefaultContent) && sDefaultContent != null && sDefaultContent.length > 0)
        columnData.sDefaultContent = sDefaultContent;

    return columnData;
}

function RefreshDataTable(tableId) {
    var table = $('#' + tableId).DataTable();
    table.ajax.reload(null, false);
}

function ResetDataTableState(tableId, resetPaging) {
    $('#' + tableId).DataTable().state.clear();
}

function dtGetObj(tableId) {
    return $('#' + tableId).DataTable();
}

function dtSelectedRowData(tableId) {
    return dtTotalRows(tableId) > 0 ? dtGetObj(tableId).rows({ selected: true }).data().shift() : null;
}

function dtSelectedRow(tableId) {
    return dtTotalRows(tableId) > 0 ? dtGetObj(tableId).rows({ selected: true }).shift() : null;
}

function dtSelectedRowsData(tableId) {
    return $('#' + tableId).DataTable().rows({ selected: true }).data().toArray();
}

function dtDeSelectRows(tableId) {
    $('#' + tableId).DataTable().rows({ selected: true }).deselect();
}

function dtSelectRow(tableId, key, value) {
    $('#' + tableId).DataTable().rows(function (idx, data, node) {
        return data[key] === value;
    }).select();
}

function dtTotalRows(tableId) {
    return dtGetObj(tableId).data().count();
}

function dtResetSelection(tableId) {
    dtGetObj(tableId).rows().deselect();
}

function dtUpdateRow(tableId, _data) {
    var selected = $('#' + tableId).find('tr.selected');
    $('#' + tableId).DataTable().row(selected).data(_data);
}

function dtAddRow(tableId, data) {
    $('#' + tableId).DataTable().row.add(data);
}

function dtRedraw(tableId) {
    dtGetObj(tableId).columns.adjust().draw(); // Redraw the DataTable
}

function dtGetAllData(tableId) {
    return $('#' + tableId).dataTable().fnGetData();
}

function dtRemoveSelectedRow(tableId) {
    $('#' + tableId).DataTable().row('.selected').remove().draw(false);
}

function dtWhere(tableId, criteria) {
    $('#' + tableId).dataTable().fnSettings().ajax.data.Where = criteria;
    RefreshDataTable(tableId);
}

function dtPopulate(tableId, source) {
    $('#' + tableId).DataTable().clear().rows.add(source).draw();
}

function dtClear(tableId) {
    $('#' + tableId).DataTable().clear().draw();
}

function dtRemoveInLineSearch(tableId) {
    $('div[id="' + tableId + '_wrapper"]').find('tr.datatable-column-filters').remove();
}

function dtAppend(tableId, source) {
    $('#' + tableId).DataTable().rows.add(source).draw();
}

function dtNotIn(tableId, field, source) {
    var criteria;
    var len = source.length;
    $.each(source, function (index, item) {
        criteria += field + ' != ' + item;
        if (index !== len - 1) {
            criteria += ' AND ';
        }
    });
    $('#' + tableId).dataTable().fnSettings().ajax.data.Where = criteria;
    RefreshDataTable(tableId);
}


function dtAdjust() {
    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
}

function dtClearFilter(tableId) {
    $('#' + tableId).dataTable().fnFilterClear();
}

function dtGetFilterText(tableId) {
    return $('#' + tableId).dataTable().fnSettings().oPreviousSearch.sSearch;
}

function dtGetPageLength(tableId) {
    return $('#' + tableId).DataTable().page.info().length;
}

function dtGetCurrentPage(tableId) {
    return $('#' + tableId).DataTable().page.info().page;
}

function dtIsServerProcessing(tableId) {
    return $('#' + tableId).DataTable().page.info().serverSide;
}

function dtGetFilteredRows(tableId, propName, propVal) {
    var result = $.grep($('#' + tableId).DataTable().rows().data().toArray(), function (item) {
        return item[propName] === propVal;
    });
    return result;
}

function dtGetDataByRow(row, tableId) {
    return $('#' + tableId).DataTable().rows(row).data().toArray().shift();
}