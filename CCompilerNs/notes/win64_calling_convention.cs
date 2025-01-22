/*
Microsoft x64 Calling Convention (used by MinGW GCC)

=====
[General Parameter Passing]

Registers Used:
The first four arguments (integer or pointer types) are passed in:
%rcx, %rdx, %r8, %r9 (in order).
Floating-point arguments are passed in:
%xmm0, %xmm1, %xmm2, %xmm3 (in order).

Stack Usage:
Any additional arguments beyond the first four are passed on the stack.
Stack arguments are pushed from right to left (the last argument is at the lowest memory address).
The caller is responsible for cleaning up the stack.

Shadow Space:
Windows uses a shadow space (also called "home space") in the stack. The caller must allocate 32 bytes of stack space for the called function to use for the first four arguments, even if those arguments are passed in registers.
This shadow space is reserved directly below the return address on the stack.

Return Values:
Integer or pointer return values are stored in %rax.
Floating-point return values are stored in %xmm0

=====
[Summary]

Argument Type	       Registers	                                               Notes
Integer/Pointers	   %rcx, %rdx, %r8, %r9	                                       First four arguments
Floating-Point	       %xmm0, %xmm1, %xmm2, %xmm3	                               First four floating-point arguments
Additional Arguments   Passed on the stack	                                       Right-to-left order
Shadow Space	       32 bytes (4 × 8 bytes) on stack	                           Reserved by the caller for callee use
Return Values	       %rax or %xmm0 Integers/pointers in %rax, floats in %xmm0
 */ 