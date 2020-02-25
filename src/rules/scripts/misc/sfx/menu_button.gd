extends BaseButton

export (AudioStream) var audio_file

func _pressed():
	var audio = AudioStreamPlayer.new()
	audio.stream = audio_file

	add_child(audio)
	audio.play()

	yield(audio, "finished")
	audio.queue_free()
