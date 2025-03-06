class_name Register extends Resource;

@export var contents : Array[Variant];

# Push a new value onto the register.
func push(value : Variant):
	contents.push_back(value);

# Return the newest value in the register.
func front() -> Variant:
	return contents.back();

# Return the oldest value in the register.
func back() -> Variant:
	return contents.front();

# Remove and return the newest value in the register.
func pop() -> Variant:
	return contents.pop_back();

# Remove and return the oldest value in the register.
func dequeue() -> Variant:
	return contents.pop_front();
