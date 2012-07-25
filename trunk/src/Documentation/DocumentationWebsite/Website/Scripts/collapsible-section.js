$(function () {
    $('section.collapsible > header h3').append('<i/>').attr('title', 'Click to toggle visibility of the section');
    $('section.collapsible > header h3').click(function () {
        var $blocks = $(this).parent().siblings(),
            $self = $(this);

        $blocks.fadeToggle('fast', 'linear', function () {
            $self.closest('section').toggleClass('collapsed');
        });
        
    });
});
