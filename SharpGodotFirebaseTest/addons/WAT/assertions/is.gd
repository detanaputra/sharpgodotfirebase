extends "assertion.gd"

static func is_AABB(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: AABB" % value
	var failed: String = "%s is not builtin: AABB" % value
	var success = value is AABB
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Array(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Array" % value as String
	var failed: String = "%s is not builtin: Array" % value as String
	var success = value is Array
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Basis(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Basis" % value
	var failed: String = "%s is not builtin: Basis" % value
	var success = value is Basis
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_bool(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: bool" % value
	var failed: String = "%s is not builtin: bool" % value
	var success = value is bool
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)
	
static func is_class_instance(instance, klass: Script, context: String) -> Dictionary:
	var passed: String = "%s is instance of class: %s" % [instance, klass]
	var failed: String = "%s is not instance of class: %s" % [instance, klass]
	var success = instance is klass
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Color(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Color" % value
	var failed: String = "%s is not builtin: Color" % value
	var success = value is Color
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Dictionary(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Dictionary" % value
	var failed: String = "%s is not builtin: Dictionary" % value
	var success = value is Dictionary
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)
	
static func is_float(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: float" % value
	var failed: String = "%s is not builtin: float" % value
	var success = value is float
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_int(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: int" % value
	var failed: String = "%s is not builtin: int" % value
	var success = value is int
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_NodePath(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: NodePath" % value
	var failed: String = "%s is not builtin: NodePath" % value
	var success = value is NodePath
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Object(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Object" % value
	var failed: String = "%s is not builtin: Object" % value
	var success = value is Object
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Plane(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Plane" % value
	var failed: String = "%s is not builtin: Plane" % value
	var success = value is Plane
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_PoolByteArray(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: PackedByteArray" % value
	var failed: String = "%s is not builtin: PackedByteArray" % value
	var success = value is PackedByteArray
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_PoolColorArray(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: PackedColorArray" % value
	var failed: String = "%s is not builtin: PackedColorArray" % value
	var success = value is PackedColorArray
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_PoolIntArray(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: PackedInt32Array" % value
	var failed: String = "%s is not builtin: PackedInt32Array" % value
	var success = value is PackedInt32Array
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_PoolRealArray(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: PackedFloat32Array" % value
	var failed: String = "%s is not builtin: PackedFloat32Array" % value
	var success = value is PackedFloat32Array
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_PoolStringArray(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: PackedStringArray" % value
	var failed: String = "%s is not builtin: PackedStringArray" % value
	var success = value is PackedStringArray
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_PoolVector2Array(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: PackedVector2Array" % value
	var failed: String = "%s is not builtin: PackedVector2Array" % value
	var success = value is PackedVector2Array
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_PoolVector3Array(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: PackedVector3Array" % value
	var failed: String = "%s is not builtin: PackedVector3Array" % value
	var success = value is PackedVector3Array
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Quat(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Quaternion" % value
	var failed: String = "%s is not builtin: Quaternion" % value
	var success = value is Quaternion
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Rect2(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Rect2" % value
	var failed: String = "%s is not builtin: Rect2" % value
	var success = value is Rect2
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_RID(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: RID" % value
	var failed: String = "%s is not builtin: RID" % value
	var success = value is RID
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_String(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: String" % value
	var failed: String = "%s is not builtin: String" % value
	var success = value is String
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Transform(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Transform3D" % value
	var failed: String = "%s is not builtin: Transform3D" % value
	var success = value is Transform3D
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Transform2D(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Transform2D" % value
	var failed: String = "%s is not builtin: Transform2D" % value
	var success = value is Transform2D
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Vector2(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Vector2" % value
	var failed: String = "%s is not builtin: Vector2" % value
	var success = value is Vector2
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)

static func is_Vector3(value, context: String) -> Dictionary:
	var passed: String = "%s is builtin: Vector3" % value
	var failed: String = "%s is not builtin: Vector3" % value
	var success = value is Vector3
	var expected = passed
	var result = passed if success else failed
	return _result(success, passed, result, context)
