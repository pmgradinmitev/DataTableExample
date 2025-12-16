$(document).ready(function () {
    $('#medicinesTable').DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Home/GetMedicines",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [4],
            "visible": true,
            "searchable": false,
            "orderable": false
        }],
        "columns": [
            { "data": "name", "name": "Name", "autoWidth": true },
            { "data": "activeIngredient", "name": "ActiveIngredient", "autoWidth": true },
            { "data": "manufacturer", "name": "Manufacturer", "autoWidth": true },
            { "data": "price", "name": "Price", "autoWidth": true },
            {
                "data": "id",
                "width": "300px",
                "render": function (data, type, row) {
                    var buttons = '';
                    buttons += ' <button type="button" class="btn btn-warning me-1">' +
                        '<i class="fas fa-edit"></i> Редактиране</button>';
                    buttons += ' <button type="button" class="btn btn-danger">' +
                        '<i class="fas fa-trash"></i> Изтриване</button>';
                    return buttons;
                }
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/bg.json'
        }
    });
});