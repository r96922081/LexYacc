
#=================================================#

#Main() -> GenerateAsm() -> EmitAsm()
.bss

.lcomm board, 64

.data

N: .quad 8
stringLiteral_0:  .asciz "Q "
stringLiteral_1:  .asciz ". "
stringLiteral_2:  .asciz "\n"
stringLiteral_3:  .asciz "Solution count = % d\n"

.text

#FunctionDeclare =>
.global is_safe
is_safe:
push %rbp
mov %rsp, %rbp
add $-8, %rsp
lea 16(%rbp), %rax
mov %rcx, (%rax)
lea 24(%rbp), %rax
mov %rdx, (%rax)
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
mov %rax, -8(%rbp)
#<= DeclareStatement
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
loop_start_0:
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
jge group_false_label_6
group_true_label_7:
jmp boolean_true_label_4
group_false_label_6:
boolean_false_label_3:
jmp loop_end_1
jmp boolean_no_jmp_label_5
boolean_true_label_4:
boolean_no_jmp_label_5:
lea board(%rip), %rbx
push %rbx
lea -8(%rbp), %rbx
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
movq $0, %rax
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
lea 24(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jne group_false_label_12
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
#ReturnStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
leave
ret
#<= ReturnStatement
jmp branch_compound_if_end_14
branch_compound_if_end_14:
lea board(%rip), %rbx
push %rbx
lea -8(%rbp), %rbx
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
movq $0, %rax
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
lea -8(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
lea 24(%rbp), %rbx
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
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jne group_false_label_19
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
#ReturnStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
leave
ret
#<= ReturnStatement
jmp branch_compound_if_end_21
branch_compound_if_end_21:
lea board(%rip), %rbx
push %rbx
lea -8(%rbp), %rbx
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
movq $0, %rax
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
lea -8(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rbx
pop %rax
add %rbx, %rax
push %rax
lea 24(%rbp), %rbx
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
pop %rbx
pop %rax
add %rbx, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jne group_false_label_26
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
#ReturnStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
leave
ret
#<= ReturnStatement
jmp branch_compound_if_end_28
branch_compound_if_end_28:
updater_2:
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
jmp loop_start_0
loop_end_1:
#<= ForLoopStatement
#ReturnStatement =>
mov $1, %rax
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
#FunctionDeclare =>
.global solve
solve:
push %rbp
mov %rsp, %rbp
add $-32, %rsp
lea 16(%rbp), %rax
mov %rcx, (%rax)
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
mov %rax, -8(%rbp)
#<= DeclareStatement
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
mov %rax, -16(%rbp)
#<= DeclareStatement
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea N(%rip), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jne group_false_label_33
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
lea N(%rip), %rbx
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
lea N(%rip), %rbx
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
lea board(%rip), %rbx
push %rbx
lea -8(%rbp), %rbx
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
movq $0, %rax
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
lea -16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jne group_false_label_56
group_true_label_57:
jmp boolean_true_label_54
group_false_label_56:
boolean_false_label_53:
jmp boolean_no_jmp_label_55
boolean_true_label_54:
jmp branch_52
boolean_no_jmp_label_55:
jmp branch_58
jmp branch_compound_if_end_59
branch_52:
#FunctionCallExpression =>
lea stringLiteral_0(%rip), %rax
push %rax
pop %rax
push %rax
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
jmp branch_compound_if_end_59
branch_58:
#FunctionCallExpression =>
lea stringLiteral_1(%rip), %rax
push %rax
pop %rax
push %rax
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
jmp branch_compound_if_end_59
branch_compound_if_end_59:
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
lea stringLiteral_2(%rip), %rax
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
#FunctionCallExpression =>
lea stringLiteral_2(%rip), %rax
push %rax
pop %rax
push %rax
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
#ReturnStatement =>
mov $1, %rax
push %rax
pop %rax
push %rax
pop %rax
leave
ret
#<= ReturnStatement
jmp branch_compound_if_end_35
branch_compound_if_end_35:
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
mov %rax, -24(%rbp)
#<= DeclareStatement
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
mov %rax, -32(%rbp)
#<= DeclareStatement
#ForLoopStatement =>
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
loop_start_60:
lea -32(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea N(%rip), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jge group_false_label_66
group_true_label_67:
jmp boolean_true_label_64
group_false_label_66:
boolean_false_label_63:
jmp loop_end_61
jmp boolean_no_jmp_label_65
boolean_true_label_64:
boolean_no_jmp_label_65:
#FunctionCallExpression =>
lea 16(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea -32(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rdx
pop %rcx
add $-32, %rsp
call is_safe
add $32, %rsp
#<= FunctionCallExpression
push %rax
pop %rax
push %rax
mov $0, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
je group_false_label_72
group_true_label_73:
jmp boolean_true_label_70
group_false_label_72:
boolean_false_label_69:
jmp boolean_no_jmp_label_71
boolean_true_label_70:
jmp branch_68
boolean_no_jmp_label_71:
jmp branch_compound_if_end_74
branch_68:
#AssignmentStatement =>
lea -32(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea board(%rip), %rbx
push %rbx
lea 16(%rbp), %rbx
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
movq $0, %rax
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
lea -24(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
#FunctionCallExpression =>
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
add %rbx, %rax
push %rax
pop %rcx
add $-32, %rsp
call solve
add $32, %rsp
#<= FunctionCallExpression
push %rax
pop %rax
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
jmp branch_compound_if_end_74
branch_compound_if_end_74:
updater_62:
#AssignmentStatement =>
lea -32(%rbp), %rbx
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
lea -32(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp loop_start_60
loop_end_61:
#<= ForLoopStatement
#ReturnStatement =>
lea -24(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
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
#FunctionDeclare =>
.global main
main:
push %rbp
mov %rsp, %rbp
add $-16, %rsp
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
mov %rax, -8(%rbp)
#<= DeclareStatement
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rax
mov %rax, -16(%rbp)
#<= DeclareStatement
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
loop_start_75:
lea -8(%rbp), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
lea N(%rip), %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jge group_false_label_81
group_true_label_82:
jmp boolean_true_label_79
group_false_label_81:
boolean_false_label_78:
jmp loop_end_76
jmp boolean_no_jmp_label_80
boolean_true_label_79:
boolean_no_jmp_label_80:
#AssignmentStatement =>
mov $-1, %rax
push %rax
pop %rax
push %rax
lea board(%rip), %rbx
push %rbx
lea -8(%rbp), %rbx
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
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
updater_77:
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
jmp loop_start_75
loop_end_76:
#<= ForLoopStatement
#AssignmentStatement =>
#FunctionCallExpression =>
mov $0, %rax
push %rax
pop %rax
push %rax
pop %rcx
add $-32, %rsp
call solve
add $32, %rsp
#<= FunctionCallExpression
push %rax
pop %rax
push %rax
lea -16(%rbp), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#FunctionCallExpression =>
lea stringLiteral_3(%rip), %rax
push %rax
pop %rax
push %rax
lea -16(%rbp), %rbx
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

