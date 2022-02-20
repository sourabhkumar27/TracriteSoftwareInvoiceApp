function ShowLineItems(InvoiceId) {
    $("#hdnInvoiceId").val(InvoiceId);
    fillLineItems();

    $("#InvoiceLineItemsModal").modal();
}

$("#btnAddNewLines").click(function (evt) {

    evt.preventDefault();

    ClearLineItemsFields();

    $("#CreateInvoiceLineItemsModal").modal();
});

function ClearLineItemsFields() {

    $("#txtDescription").val("");
    $("#txtQuantity").val("");
    $("#txtUnitPrice").val("");
}


function fillLineItems() {

    var html = "";
    $("#tbodyLineItems").html("");

    //var table = $('#tblLineItems').DataTable();
    //table.destroy();

    var InvoiceId = $("#hdnInvoiceId").val();
    var lineItems = new FormData();
    lineItems.append('InvoiceId', InvoiceId);

    $.ajax({
        url: '/LineItems/GetLineItems',
        type: "POST",
        contentType: false,
        processData: false,
        dataType: 'json',
        data: lineItems,

        success: function (result) {
            if (result.lineItems != null && result.lineItems != "") {

                for (var i = 0; i < result.lineItems.length; i++) {

                    html += "<tr>";

                    html += "<td>" + result.lineItems[i].description + "</td>";
                    html += "<td>" + result.lineItems[i].quantity + "</td>";
                    html += "<td>" + result.lineItems[i].unitPrice + "</td>";

                    var qty = parseFloat(result.lineItems[i].quantity);
                    var unitPrice = parseFloat(result.lineItems[i].unitPrice);
                    var total = qty * unitPrice;

                    html += "<td>" + total + "</td>";

                    html += "<td><a href='#' onclick='EditLineItems(" + result.lineItems[i].id + ",event)'>Edit</a>|<a href='#' onclick='DeleteLineItems(" + result.lineItems[i].id + ",event)'>Delete</a></td>";
                }
                html += "</tr>";
            }

            $("#tbodyLineItems").html(html);
            // $('#tblLineItems').DataTable();
        },
        error: function (e) {

            return false;
        }
    });
}


$("#btnSaveLineItems").click(function (evt) {

    evt.preventDefault();

    var Id = $("#hdnLineItemsId");
    var InvoiceId = $("#hdnInvoiceId");
    var Description = $("#txtDescription");
    var Quantity = $("#txtQuantity");
    var UnitPrice = $("#txtUnitPrice");

    if (Description.val() == "") {
        alert("Description Required");
        Description.focus();
        return false;
    }
    else if (Quantity.val() == "") {
        alert("Quantity Required");
        Quantity.focus();
        return false;
    }
    else if (UnitPrice.val() == "") {
        alert("Unit Price Required");
        UnitPrice.focus();
        return false;
    }
    else {

        var lineItem = new FormData();
        lineItem.append('Id', Id.val());
        lineItem.append('InvoiceId', InvoiceId.val());
        lineItem.append('Description', Description.val());
        lineItem.append('Quantity', Quantity.val());
        lineItem.append('UnitPrice', UnitPrice.val());

        $.ajax({
            url: '/LineItems/SaveLineItems',
            type: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            data: lineItem,

            success: function (result) {

                if (result.ok) {

                    if (result.msg != "") {
                        alert(result.msg);
                    }
                    ClearLineItemsFields();
                    fillLineItems();
                    $("#CreateInvoiceLineItemsModal").modal("toggle");
                }
                else {
                    if (result.msg != "") {
                        alert(result.msg);
                    }
                }

                if (result.newurl != "") {
                    window.location.href = result.newurl;
                }
            },
            error: function (e) {

                return false;
            }
        });
    }
});



function EditLineItems(id) {

    $("#hdnLineItemsId").val(id);

    var Id = id;
    var lineItems = new FormData();
    lineItems.append('Id', Id);

    $.ajax({
        url: '/LineItems/GetLineItems',
        type: "POST",
        contentType: false,
        processData: false,
        dataType: 'json',
        data: lineItems,

        success: function (result) {

            if (result.lineItems != null && result.lineItems != "") {

                if (result.lineItems.length > 0) {
                    $("#txtDescription").val(result.lineItems[0].description);
                    $("#txtQuantity").val(result.lineItems[0].quantity);
                    $("#txtUnitPrice").val(result.lineItems[0].unitPrice);
                    $("#CreateInvoiceLineItemsModal").modal();
                }
            }
        },
        error: function (e) {

            return false;
        }
    });
}


function DeleteLineItems(id) {

    var Id = id;
    var lineItems = new FormData();
    lineItems.append('Id', Id);

    $.ajax({
        url: '/LineItems/DeleteLineItems',
        type: "POST",
        contentType: false,
        processData: false,
        dataType: 'json',
        data: lineItems,

        success: function (result) {

            if (result.ok) {

                if (result.msg != "") {
                    alert(result.msg);
                }
                fillLineItems();
            }
            else {
                if (result.msg != "") {
                    alert(result.msg);
                }
            }
        },
        error: function (e) {

            return false;
        }
    });
}