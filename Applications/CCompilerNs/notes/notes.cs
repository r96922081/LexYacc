/*

1. expression saves (only one) result in stack top
2. statement saves result in %rax

====

function call stack:

                stack:
                param6
                param5
                shadow space 32 bytes
                return address
new %rbp     -> old %rbp (to set as %rsp after ret)
new %rbp - 8 -> local1
new %rbp - 16-> local2

====

struct layout in stack

struct A {
   int a1;
   char a2;
   int a3;
};

A a;
a.a1 = 1;
a.a2 = 2;
a.a3 = 3;

address of a = -24(%rbp)

-12(%rbp) a.a3
-18(%rbp) a.a2
-24(%rbp) a.a1  


====
leave: equivalent to 
  mov   %ebp, %esp
  pop   %ebp
====
call: will push return address to stack and jmp to the function
====
.data, .bss

.data: initialized globals
.bss: uninitialized globals

Directive	Section	Size (Bytes)	Purpose

.byte	.data	1	Initialize 1 byte
.word	.data	2	Initialize 2 bytes
.long	.data	4	Initialize 4 bytes
.quad	.data	8	Initialize 8 bytes
.asciz	.data	Variable	Null-terminated string
.ascii	.data	Variable	Non-null-terminated string
.float	.data	4	Single-precision float
.double	.data	8	Double-precision float
.lcomm	.bss	Variable	Reserve uninitialized local memory
.comm	.bss	Variable	Reserve global uninitialized memory. "global": visible to the linker and can be accessed from other object files

--
.section .data
message:  .asciz "Hello, world!"
--
.section .data
long_var: .quad 1234567890123456789  # Allocates 8 bytes for a 64-bit value

.text
mov $23, long_var(%rip)
mov long_var(%rip), %rax
--
.section .bss
.lcomm v2, 8 # Uninitialized (set to all 0), reserved 8 bytes

.text
mov $77, v2(%rip)
mov v2(%rip), %rax

--


 */ 