function EditPanelToggle(value) {

    if (value == 'show') {
        $("#editPanel").animate({ "opacity": "1", "left": "30%" }, 500, "swing");
    }
    else {
        $("#editPanel").animate({ "opacity": "0", "left": "150%" }, 500, "swing");

        globalHiddenHolderUid = "";
        globalHiddenHolderBD = "";
        globalHiddenCardId = "";
        globalHiddenCardFiolat = "";
        EditCardCancelClick();
    }
}

function AddHolder() {
    var f = document.getElementById("nHolderF");
    var i = document.getElementById("nHolderI");
    var o = document.getElementById("nHolderO");
    var bd = document.getElementById("nHolderBD");
    var dateStr = new Date(bd.value).toISOString();
    var wp = document.getElementById("nHolderWP");

    var addHolderJSON = "{ \"uid\": \"placeholder\", \"f\": \"" + f.value + "\", \"i\": \"" + i.value + "\", \"o\": \"" + o.value + "\", \"birthhday\": \"" + dateStr + "\", \"workplace\": \"" + wp.value + "\" }";

    var settings = {
        "url": globalPublicAddress+"/api/HoldersController/AddHolder",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": addHolderJSON,
    };

    $.ajax(settings).done(function (response, msg, xhr) {
        if (xhr.status == "200") {
            alert("Держатель добавлен")
        }
        else {
            alert(xhr.status + ": " + msg)
        }
    });

    f.value = "";
    i.value = "";
    o.value = "";
    bd.value = "";
    wp.value = "";
}

function EditCardClick(e, cell) {
    globalHiddenCardId = cell.getRow().getData().Id;
    globalHiddenCardFiolat = cell.getRow().getData().Fiolat;

    var cardNum = document.getElementById("cardNum");
    var cardAcc = document.getElementById("cardAcc");
    var expire = document.getElementById("expire");
    var cvc = document.getElementById("cvc");

    var date = new Date(cell.getRow().getData().Expire);
    var year = date.getFullYear();
    var month = date.getMonth();
    var day = date.getDay();
    var formattedDate = year + "-" + String(month).padStart(2, '0') + "-" + String(day).padStart(2, '0');

    cardNum.value = cell.getRow().getData().CardNumber;
    cardAcc.value = cell.getRow().getData().CardAccount;
    expire.value = formattedDate;
    cvc.value = cell.getRow().getData().Cvc;

    var buttonAddCard = document.getElementById("addCardRow")
    buttonAddCard.style.display = "none";

    var buttonUpdateCard = document.getElementById("updateCardRow")
    buttonUpdateCard.style.display = "table-row";
}

function EditCardCancelClick() {
    globalHiddenCardId = "";

    cardNum.value = "";
    cardAcc.value = "";
    expire.value = "";
    cvc.value = "";

    var buttonAddCard = document.getElementById("addCardRow")
    buttonAddCard.style.display = "table-row";

    var buttonUpdateCard = document.getElementById("updateCardRow")
    buttonUpdateCard.style.display = "none";
}

function UpdateCard() {
    var cardNum = document.getElementById("cardNum");
    var cardAcc = document.getElementById("cardAcc");
    var expire = document.getElementById("expire");
    var cvc = document.getElementById("cvc");

    var dateStr = new Date(expire.value).toISOString();

    var updateCardJSON = "{ \"id\": " + globalHiddenCardId + ", \"uid\": \"" + globalHiddenHolderUid + "\", \"fiolat\": \"" + globalHiddenCardFiolat + "\", \"cardNumber\": \"" + cardNum.value + "\", \"cardAccount\": \"" + cardAcc.value + "\", \"expire\": \"" + dateStr + "\", \"cvc\": \"" + cvc.value + "\" }";

    var settings = {
        "url": globalPublicAddress+"/api/CardsController/UpdateCard",
        "method": "PUT",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": updateCardJSON,
    };

    $.ajax(settings).done(function () {
        GetHolderCards(globalHiddenHolderUid);
    });
}

function AddCard() {
    var cardNum = document.getElementById("cardNum");
    var cardAcc = document.getElementById("cardAcc");
    var expire = document.getElementById("expire");
    var cvc = document.getElementById("cvc");

    var dateStr = new Date(expire.value).toISOString();

    var addCardJSON = "{ \"id\": null, \"uid\": \"" + globalHiddenHolderUid + "\", \"fiolat\": \"placeholder\", \"cardNumber\": \"" + cardNum.value + "\", \"cardAccount\": \"" + cardAcc.value + "\", \"expire\": \"" + dateStr + "\", \"cvc\": \"" + cvc.value + "\" }";

    var settings = {
        "url": globalPublicAddress+"/api/CardsController/AddCard",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": addCardJSON,
    };

    $.ajax(settings).done(function (response, msg, xhr) {
        if (xhr.status == "200") {
            GetHolderCards(globalHiddenHolderUid);
        }
        else {
            alert(xhr.status + ": " + msg)
        }
    });

}

function DeleteCardClick(e, cell) {
    var settings = {
        "url": globalPublicAddress+"/api/CardsController/DeleteCard",
        "method": "DELETE",
        "timeout": 0,
        "headers": {
            "id": String(cell.getRow().getData().Id)
        },
    };

    $.ajax(settings).done(function (response, msg, xhr) {
        if (xhr.status == "200") {
            GetHolderCards(globalHiddenHolderUid);
        }
        else {
            alert(xhr.status + ": " + msg)
        }
    });
}

function GetHolderCards(uid) {
    var settings = {
        "url": globalPublicAddress+"/api/CardsController/GetHolderCards",
        "method": "GET",
        "timeout": 0,
        "headers": {
            "UID": uid,
        },
    };

    $.ajax(settings).done(function (response, msg, xhr) {
        if (xhr.status == "200") {
            if (response != "[]") {

                var editIcon = function () {
                    return "<img title=\"edit\" class=\"icon unselectable\" src=\"/images/edit.svg\"/>";
                };

                var deleteIcon = function () {
                    return "<img title=\"delete\" class=\"icon unselectable\" src=\"/images/delete.svg\"/>";
                };

                var table = new Tabulator('#cardsTable', {
                    data: JSON.parse(response),
                    layout: 'fitColumns',
                    columns: [
                        { title: 'ID', field: 'Id', visible: false },
                        { title: 'UID', field: 'Uid', visible: false },
                        { title: 'ФИО Лат.', field: 'Fiolat', resizable: false },
                        { title: 'Номер карты', field: 'CardNumber', resizable: false },
                        { title: 'Номер счета', field: 'CardAccount', resizable: false },
                        { width: 80, title: 'CVC', field: 'Cvc', resizable: false },
                        { width: 200, title: 'Срок окончания', field: 'Expire', resizable: false },
                        { width: 20, formatter: editIcon, resizable: false, cellClick: EditCardClick },
                        { width: 20, formatter: deleteIcon, resizable: false, cellClick: DeleteCardClick },
                    ]
                });
            }
        }
        else {
            alert(xhr.status + ": " + msg)
        }
    });
}

function UpdateHolder() {
    var holderF = document.getElementById("holderF");
    var holderI = document.getElementById("holderI");
    var holderO = document.getElementById("holderO");
    var holderWP = document.getElementById("holderWP");


    var updateHolderJSON = "{ \"uid\": \"" + globalHiddenHolderUid + "\", \"f\": \"" + holderF.value + "\", \"i\": \"" + holderI.value + "\", \"o\": \"" + holderO.value + "\", \"birthDate\": \"" + globalHiddenHolderBD + "\", \"workPlace\": \"" + holderWP.value + "\" }";

    var settings = {
        "url": globalPublicAddress+"/api/HoldersController/UpdateHolder",
        "method": "PUT",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": updateHolderJSON,
    };

    $.ajax(settings).done(function () {
        CurrentPage(0);
        GetHolderCards(globalHiddenHolderUid);
        EditCardCancelClick();

        var table = Tabulator.findTable("#cardsTable")[0];
        var tableData = table.getData();

        globalHiddenCardFiolat = tableData[0].Fiolat;
    });
}

function EditClick(e, cell) {
    globalHiddenHolderUid = cell.getRow().getData().Uid;
    globalHiddenHolderBD = cell.getRow().getData().BirthDate;
    GetHolderCards(globalHiddenHolderUid);

    var holderF = document.getElementById("holderF");
    var holderI = document.getElementById("holderI");
    var holderO = document.getElementById("holderO");
    var holderWP = document.getElementById("holderWP");

    holderF.value = cell.getRow().getData().F;
    holderI.value = cell.getRow().getData().I;
    holderO.value = cell.getRow().getData().O;
    holderWP.value = cell.getRow().getData().WorkPlace;

    EditPanelToggle("show");
}

function DeleteClick(e, cell) {
    var settings = {
        "url": globalPublicAddress+"/api/HoldersController/DeleteHolder",
        "method": "DELETE",
        "timeout": 0,
        "headers": {
            "uid": String(cell.getRow().getData().Uid)
        },
    };

    $.ajax(settings).done(function (response, msg, xhr) {
        if (xhr.status == "200") {
            CurrentPage(0);
        }
        else {
            alert(xhr.status + ": " + msg)
        }
    });
}

function GetPagesCount() {
    var settings = {
        "url": globalPublicAddress+"/api/LargeDataController/GetHoldersPagesCount",
        "method": "GET",
        "timeout": 0,
        "headers": {
            "pageRowsNumber": Number(rowsOnPage.value)
        },
    };

    $.ajax(settings).done(function (response) {
        if (response != "0") {
            globalPagesCnt = Number(response);
        }
        else {
            globalPagesCnt = 1;
        }
    });
}

function CurrentPage(value) {
    var rowsOnPage = document.getElementById("rowsOnPage");
    var curPage = document.getElementById("curPage");
    var pageNum = Number(curPage.value) + value;

    //GetPagesCount();

    if (rowsOnPage.value < 1) {
        rowsOnPage.value = 1;
    }

    if (rowsOnPage.value > 999) {
        rowsOnPage.value = 999;
    }

    if (curPage.value > globalPagesCnt) {
        curPage.value = globalPagesCnt
    }

    if (pageNum > 0 && pageNum < 1000) {
        var settings = {
            "url": globalPublicAddress+"/api/LargeDataController/GetHoldersDataPage",
            "method": "GET",
            "timeout": 0,
            "headers": {
                "pageNumber": pageNum - 1,
                "pageRowsNumber": Number(rowsOnPage.value)
            },
        };

        $.ajax(settings).done(function (response) {
            if (response != "[]") {
                curPage.value = pageNum;

                var editIcon = function () {
                    return "<img title=\"edit\" class=\"icon unselectable\" src=\"/images/edit.svg\"/>";
                };

                var deleteIcon = function () {
                    return "<img title=\"delete\" class=\"icon unselectable\" src=\"/images/delete.svg\"/>";
                };

                var table = new Tabulator('#holdersTable', {
                    data: JSON.parse(response),
                    layout: 'fitColumns',
                    movableColumns: true,
                    resizableColumnFit: true,
                    columns: [
                        { title: 'UID', field: 'Uid', visible: false },
                        { title: 'Фамилия', field: 'F', resizable: false },
                        { title: 'Имя', field: 'I', resizable: false },
                        { title: 'Отчество', field: 'O', resizable: false },
                        { width: 200, title: 'Дата рождения', field: 'BirthDate', resizable: false },
                        { width: 300, title: 'Место работы', field: 'WorkPlace', resizable: false },
                        { width: 20, formatter: editIcon, resizable: false, cellClick: EditClick },
                        { width: 20, formatter: deleteIcon, resizable: false, cellClick: DeleteClick },
                    ]
                });
            }
        });
    }
}