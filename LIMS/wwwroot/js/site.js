﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function createToast(type, text) {
    $("#main-container").prepend(`
                <div id="toast-generic" class="toast" role="${type}" aria-live="assertive" aria-atomic="true">
                    <div class="toast-header">
                        <strong class="mr-auto">Error</strong>
                        <button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="toast-body">
                        ${text}
                    </div>
                </div>
            `);

    $(".toast").toast({ delay: 7000 });
    $('.toast').on('hidden.bs.toast', function () {
        $(this).remove();
    })
    $(".toast").toast('show');
}