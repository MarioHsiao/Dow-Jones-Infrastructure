$(function () {
    $('section.collapsible > h3').append('<i/>');
    $('section.collapsible > h3').click(function () {
        var $blocks = $(this).siblings(),
            $self = $(this);

        $blocks.toggle(800, 'linear', function () {
            $self.closest('section').toggleClass('collapsed');
        });
        
    });
});
