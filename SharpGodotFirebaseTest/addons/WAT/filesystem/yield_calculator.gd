extends RefCounted

const COMMENT: String = "#"
const PRE_HOOK: String = "func pre()"
const POST_HOOK: String = "func post()"
const FUNCTION: String = "func"
const OPENING_STRING_QUOTE: String = '"'

static func calculate_yield_time(gdscript: Script, test_method_count: int) -> float:
	var time: float = 0.0
	var floats: PackedFloat32Array = PackedFloat32Array()
	var in_hook: bool = false
	for line in gdscript.source_code.split("\n"):
		if line.begins_with(PRE_HOOK) or line.begins_with(POST_HOOK):
			# Yielding pre or post requires counting for each test method
			in_hook = true
		elif line.begins_with(FUNCTION):
			in_hook = false
		if "YIELD" in line and not line.begins_with(COMMENT) and not line.begins_with(OPENING_STRING_QUOTE):
			var f: PackedFloat32Array = PackedFloat32Array()
			f += line.split_floats("(")
			f += line.split_floats(",")
			if in_hook:
				f = duplicate(f, test_method_count)
			floats += f
	for real in floats:
		time += real
	return time

static func duplicate(source: PackedFloat32Array, count: int) -> PackedFloat32Array:
	var floats: PackedFloat32Array = PackedFloat32Array()
	for real in source:
		for i in count:
			floats.append(real)
	return floats
