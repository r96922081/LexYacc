
#=================================================#

#Main() -> GenerateAsm() -> EmitAsm()
.bss

.lcomm magicSquare, 1800

.data

stringLiteral_83:  .asciz "Magic Square of size %d:\n"
stringLiteral_84:  .asciz "%4d"
stringLiteral_85:  .asciz "\n"

.text

#FunctionDeclare =>
.global generateMagicSquare
generateMagicSquare:
push %rbp
mov %rsp, %rbp
add $-40, %rsp
lea 16(%rbp), %rax
mov %rcx, (%rax)
#DeclareStatement =>
mov %rax, -8(%rbp)
#<= DeclareStatement
#DeclareStatement =>
mov %rax, -16(%rbp)
#<= DeclareStatement
#DeclareStatement =>
mov %rax, -24(%rbp)
#<= DeclareStatement
#DeclareStatement =>
mov %rax, -32(%rbp)
#<= DeclareStatement
#DeclareStatement =>
mov $1, %rax
push %rax
pop %rax
push %rax
pop %rax
mov %rax, -40(%rbp)
#<= DeclareStatement
#AssignmentStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
lea -8(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $2, %rax
push %rax
pop %rbx
pop %rax
movq $0, %rdx
div %rbx
push %rax
pop %rax
push %rax
lea -16(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#WhileLoopStatement =>
loop_start_0:
lea -40(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rbx
pop %rax
mul %rbx
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jg group_false_label_6
group_true_label_7:
jmp boolean_true_label_4
group_false_label_6:
boolean_false_label_3:
jmp loop_end_1
jmp boolean_no_jmp_label_5
boolean_true_label_4:
boolean_no_jmp_label_5:
#AssignmentStatement =>
lea -40(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea magicSquare(%rip), %rbx
push %rbx
lea -16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -8(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rax
mov $15, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -40(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
add %rbx, %rax
push %rax
lea -40(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -8(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
lea -24(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
add %rbx, %rax
push %rax
lea -32(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
lea -24(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jge group_false_label_12
group_true_label_13:
jmp boolean_true_label_10
group_false_label_12:
boolean_false_label_9:
jmp boolean_no_jmp_label_11
boolean_true_label_10:
jmp branch_8
boolean_no_jmp_label_11:
jmp branch_compound_if_end_14
branch_8:
#AssignmentStatement =>
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
lea -24(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_14
branch_compound_if_end_14:
lea -32(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jl group_false_label_19
group_true_label_20:
jmp boolean_true_label_17
group_false_label_19:
boolean_false_label_16:
jmp boolean_no_jmp_label_18
boolean_true_label_17:
jmp branch_15
boolean_no_jmp_label_18:
jmp branch_compound_if_end_21
branch_15:
#AssignmentStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
lea -32(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_21
branch_compound_if_end_21:
lea magicSquare(%rip), %rbx
push %rbx
lea -32(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -24(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rax
mov $15, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
je group_false_label_26
group_true_label_27:
jmp boolean_true_label_24
group_false_label_26:
boolean_false_label_23:
jmp boolean_no_jmp_label_25
boolean_true_label_24:
jmp branch_22
boolean_no_jmp_label_25:
jmp branch_compound_if_end_28
branch_22:
#AssignmentStatement =>
lea -8(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
add %rbx, %rax
push %rax
lea -24(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
lea -24(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jl group_false_label_33
group_true_label_34:
jmp boolean_true_label_31
group_false_label_33:
boolean_false_label_30:
jmp boolean_no_jmp_label_32
boolean_true_label_31:
jmp branch_29
boolean_no_jmp_label_32:
jmp branch_compound_if_end_35
branch_29:
#AssignmentStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
lea -24(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_35
branch_compound_if_end_35:
#AssignmentStatement =>
lea -16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea -32(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_28
branch_compound_if_end_28:
#AssignmentStatement =>
lea -24(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea -8(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -32(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea -16(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
updater_2:
jmp loop_start_0
loop_end_1:
#<= WhileLoopStatement
#FunctionCallExpression =>
lea stringLiteral_83(%rip), %rax
push %rax
pop %rax
push %rax
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rdx
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
#ForLoopStatement =>
#AssignmentStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
lea -8(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
loop_start_36:
lea -8(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jge group_false_label_42
group_true_label_43:
jmp boolean_true_label_40
group_false_label_42:
boolean_false_label_39:
jmp loop_end_37
jmp boolean_no_jmp_label_41
boolean_true_label_40:
boolean_no_jmp_label_41:
#ForLoopStatement =>
#AssignmentStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
lea -16(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
loop_start_44:
lea -16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jge group_false_label_50
group_true_label_51:
jmp boolean_true_label_48
group_false_label_50:
boolean_false_label_47:
jmp loop_end_45
jmp boolean_no_jmp_label_49
boolean_true_label_48:
boolean_no_jmp_label_49:
#FunctionCallExpression =>
lea stringLiteral_84(%rip), %rax
push %rax
pop %rax
push %rax
lea magicSquare(%rip), %rbx
push %rbx
lea -16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -8(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rax
mov $15, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rdx
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
updater_46:
#AssignmentStatement =>
lea -16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
add %rbx, %rax
push %rax
lea -16(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp loop_start_44
loop_end_45:
#<= ForLoopStatement
#FunctionCallExpression =>
lea stringLiteral_85(%rip), %rax
push %rax
pop %rax
push %rax
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
updater_38:
#AssignmentStatement =>
lea -8(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
add %rbx, %rax
push %rax
lea -8(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp loop_start_36
loop_end_37:
#<= ForLoopStatement
leave
ret
#<= FunctionDeclare
#FunctionDeclare =>
.global main
main:
push %rbp
mov %rsp, %rbp
add $0, %rsp
#FunctionCallExpression =>
mov $7, %rax
push %rax
pop %rax
push %rax
pop %rcx
add $-32, %rsp
call generateMagicSquare
add $32, %rsp
#<= FunctionCallExpression
#ReturnStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
leave
ret
#<= ReturnStatement
leave
ret
#<= FunctionDeclare

