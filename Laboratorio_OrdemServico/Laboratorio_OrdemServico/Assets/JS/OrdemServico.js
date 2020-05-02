$(document).ready(function () {

    var countnomeExame = 0;

    $("#btnAddExame").on("click", function () {
        var nomeExame = GetSelectedText("ExameId");
        var idExame = GetSelectedValue("ExameId");
        var list = $("#tblExamesAdicionados tbody").html();
        var line = $("#tblExamesAdicionados tbody tr").length;
        countnomeExame = line;
        var value = "";
        var index = 0;
        var nomeExameExit = false;
        var nomeExameValue = "";
        var idline = "";


        if (nomeExame != "" && nomeExame != null) {

            for (var i = 0; i < line; i++) {
                index = i + 1;
                value = $("#tblExamesAdicionados > tbody > tr:nth-child(" + index + ") > td:nth-child(2)").html().split("<")[0].trim();

                if (nomeExame == value) {
                    nomeExameExit = true;
                }
            }

            if (nomeExameExit == false) {

                list += "<tr id = 'line-" + countnomeExame + "'>" +
                    "<td>" + nomeExame +
                    "<input type='hidden' name = 'OrdemServicoExame[" + countnomeExame + "].ExameId' value='" + idExame + "' />" +
                    "<input type='hidden' name = 'OrdemServicoExame[" + countnomeExame + "].OrdemServicoId' value='0' />" +
                    "<input type='hidden' name = 'OrdemServicoExame[" + countnomeExame + "].OrdemServicoExameId' value='0' />" +
                    "</td>" +
                    "<td><i class='fas fa-trash-alt' data-target='#line-" + countnomeExame + "'></i></td>" +
                    "</tr>";

                $("#tblExamesAdicionados tbody").html(list);
                $("#nomeExame").val("");

                list = "";
                line = $("#tblExamesAdicionados tbody tr").length;

                for (var i = 0; i < line; i++) {
                    index = i + 1;

                    idline = "#" + $("#tblExamesAdicionados > tbody:nth-child(2) > tr:nth-child(" + index + ")").attr("id");
                    $(idline + "input").attr("name", "OrdemServicoExame[" + i + "].ExameId");
                }

                countnomeExame++;
            }
            else {
                //var warning = resourcesJS["msgUnidadeJaInserida"];
                //showAlert("warning", warning, resourcesJS["Atencao"]);
            }
        }
        else {
            //var warning = resourcesJS["msgCampoEmBranco"];
            //showAlert("warning", warning, resourcesJS["Atencao"]);
        }
    });

    $("body").on("click", "#tblExamesAdicionados .fa-trash-alt", function () {
        var idLine = $(this).attr("data-target");
        var line = 0;
        var index = 0;

        if (idLine != "" && idLine != undefined && idLine != null) {
            line = $("#tblExamesAdicionados tbody tr").length;
            $(idLine).remove();

            for (var i = 0; i < line; i++) {
                index = i + 1;

                idline = "#" + $("#tblExamesAdicionados > tbody:nth-child(2) > tr:nth-child(" + index + ")").attr("id");
                if (idline != "#undefined") {
                    $(idline + " input")[0].name = "OrdemServicoExame[" + i + "].ExameId";
                    $(idline + " input")[1].name = "OrdemServicoExame[" + i + "].OrdemServicoId";
                    $(idline + " input")[2].name = "OrdemServicoExame[" + i + "].OrdemServicoExameId";
                    $(idline + " i").attr("data-target", "#line-" + i);
                    $(idline).attr("id", "line-" + i);
                }
            }
        }
    });
});

function GetSelectedValue(id) {
    var e = document.getElementById(id);
    var result = e.options[e.selectedIndex].value;

    return result;
}

function GetSelectedText(id) {
    var e = document.getElementById(id);
    var result = e.options[e.selectedIndex].text;

    return result;
}