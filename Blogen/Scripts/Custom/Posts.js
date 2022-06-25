var dataTable;

$(document).ready(
    function () {
        loadDataTable();

        var myModal = new bootstrap.Modal(document.getElementById('addPostModal'), {
            keyboard: true
        });

        $("#btnMySubmitPost").click(
            function () {

                for (var i in CKEDITOR.instances) {
                    CKEDITOR.instances[i].updateElement();
                };
                var myformData = new FormData();
                myformData.append("title", $("#title").val());
                myformData.append("CatID", $("#CatID").val());
                myformData.append("Body", $("#Body").val());
                var filebase = $("#PostImage").get(0);
                var files = filebase.files;
                myformData.append("PostImage", files[0]);
                //console.log("Body values is :" + $("#Body").val());
                //console.log(myformData);
                $.ajax(
                    {
                        type: 'POST',
                        url: '/Posts/Create',
                        contentType: false,
                        processData: false,
                        data: myformData,
                        success: function () {
                            myModal.hide();
                            dataTable.ajax.reload();
                        }
                    }
                )
            }
        );
    }
)

function loadDataTable() {

    try {
        const dataTable = $("#tblPosts").DataTable(
            {
                "ajax": {
                    "url": "/Posts/GetAll"
                },
                "columns": [
                    /*                { "data": "ID" },*/
                    { "data": "Title" },
                    { "data": "Category" },
                    { "data": "UpdateDate" },
                    {
                        "data": "ID",
                        "render": function (data) {
                            return `
                                <a href="/Posts/Details/${data}" class="btn btn-secondary">
                                    <i class="fa fa-angle-double-right"></i>  Details
                                </a>
                                `
                        }
                    }
                ]
            }
        );
    } catch (e) {
        console.log(e)
    }
}
