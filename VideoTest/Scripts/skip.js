function seek() {
    var player = videojs("current_video");
    player.play();
    player.currentTime(100);
}