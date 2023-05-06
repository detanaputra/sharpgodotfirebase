extends RefCounted

static func load_metadata(filesystem: RefCounted) -> void:
	var path: String
	if not filesystem.Settings.metadata_directory():
		path = filesystem.Settings.test_directory()
	else:
		path = filesystem.Settings.metadata_directory()
	path += "/metadata.json"
	if not DirAccess.new().file_exists(path):
		return

	var file = File.new()
	file.open(path, File.READ)
	var test_json_conv = JSON.new()
	test_json_conv.parse(file.get_as_text()).result
	var content: Dictionary = test_json_conv.get_data()
	file.close()
	
	for key in content:
		if key == "failed":
			filesystem.failed.paths = content[key]
		else:
			filesystem.tagged.tagged[key] = content[key]
		
static func save_metadata(filesystem: RefCounted) -> void:
	var path: String
	if not filesystem.Settings.metadata_directory():
		push_warning("WAT: Cannot find metadata directory. Defaulting to test directory to save metadata")
		path = filesystem.Settings.test_directory()
	else:
		path = filesystem.Settings.metadata_directory()
	if not DirAccess.new().dir_exists(path):
		DirAccess.new().make_dir_recursive(path)
		
	path += "/metadata.json"
	var data = {"failed": filesystem.failed.paths}
	for tag in filesystem.tagged.tagged:
		var paths: Array = filesystem.tagged.tagged[tag]
		if not paths.is_empty():
			data[tag] = paths
	
	var file = File.new()
	file.open(path, File.WRITE)
	file.store_string(JSON.stringify(data, "\t", true))
	file.close()
