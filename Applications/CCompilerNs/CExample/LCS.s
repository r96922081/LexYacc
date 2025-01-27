
#=================================================#

#Main() -> GenerateAsm() -> EmitAsm()
.data

stringLiteral_0:  .asciz "Length of Longest Common Subsequence of %s, %s : % d\n"
stringLiteral_1:  .asciz "Longest Common Subsequence: %s\n"
stringLiteral_2:  .asciz "AGGTABWZ"
stringLiteral_3:  .asciz "GXTXAYBYZ"

.text

#FunctionDeclare =>
.global lcs
lcs:
push %rbp
mov %rsp, %rbp
add $-856, %rsp
lea 16(%rbp), %rax
mov %rcx, (%rax)
lea 24(%rbp), %rax
mov %rdx, (%rax)
lea 32(%rbp), %rax
mov %r8, (%rax)
lea 40(%rbp), %rax
mov %r9, (%rax)
#DeclareStatement =>
#<= DeclareStatement
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
mov %rax, -808(%rbp)
#<= DeclareStatement
#DeclareStatement =>
#<= DeclareStatement
#DeclareStatement =>
#<= DeclareStatement
#DeclareStatement =>
#<= DeclareStatement
#DeclareStatement =>
#<= DeclareStatement
#DeclareStatement =>
#<= DeclareStatement
#ForLoopStatement =>
#AssignmentStatement =>
mov $0, %rax
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
loop_start_0:
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea 32(%rbp), %rbx # param, m
push %rbx
pop %rbx
mov (%rbx), %rax
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
#ForLoopStatement =>
#AssignmentStatement =>
mov $0, %rax
push %rax
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
loop_start_8:
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea 40(%rbp), %rbx # param, n
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jg group_false_label_14
group_true_label_15:
jmp boolean_true_label_12
group_false_label_14:
boolean_false_label_11:
jmp loop_end_9
jmp boolean_no_jmp_label_13
boolean_true_label_12:
boolean_no_jmp_label_13:
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $0, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jne group_false_label_20
group_true_label_21:
jmp boolean_true_label_18
group_false_label_20:
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $0, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jne group_false_label_22
group_true_label_23:
jmp boolean_true_label_18
group_false_label_22:
boolean_false_label_17:
jmp boolean_no_jmp_label_19
boolean_true_label_18:
jmp branch_16
boolean_no_jmp_label_19:
jmp branch_24
jmp branch_compound_if_end_25
branch_16:
#AssignmentStatement =>
mov $0, %rax
push %rax
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $10, %rcx
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
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_25
branch_24:
lea 16(%rbp), %rbx # param, str1[]
mov (%rbx), %rbx
push %rbx
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $1, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
movzbq (%rbx), %rax
push %rax
lea 24(%rbp), %rbx # param, str2[]
mov (%rbx), %rbx
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $1, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
movzbq (%rbx), %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jne group_false_label_30
group_true_label_31:
jmp boolean_true_label_28
group_false_label_30:
boolean_false_label_27:
jmp boolean_no_jmp_label_29
boolean_true_label_28:
jmp branch_26
boolean_no_jmp_label_29:
jmp branch_32
jmp branch_compound_if_end_33
branch_26:
#AssignmentStatement =>
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $10, %rcx
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
mov $1, %rax
push %rax
pop %rbx
pop %rax
add %rbx, %rax
push %rax
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $10, %rcx
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
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_33
branch_32:
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $10, %rcx
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
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $10, %rcx
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
pop %rbx
pop %rax
cmp %rbx, %rax
jle group_false_label_38
group_true_label_39:
jmp boolean_true_label_36
group_false_label_38:
boolean_false_label_35:
jmp boolean_no_jmp_label_37
boolean_true_label_36:
jmp branch_34
boolean_no_jmp_label_37:
jmp branch_40
jmp branch_compound_if_end_41
branch_34:
#AssignmentStatement =>
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $10, %rcx
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
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $10, %rcx
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
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_41
branch_40:
#AssignmentStatement =>
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $10, %rcx
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
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $10, %rcx
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
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_41
branch_compound_if_end_41:
jmp branch_compound_if_end_33
branch_compound_if_end_33:
jmp branch_compound_if_end_25
branch_compound_if_end_25:
updater_10:
#AssignmentStatement =>
lea -840(%rbp), %rbx # local, j
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
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp loop_start_8
loop_end_9:
#<= ForLoopStatement
updater_2:
#AssignmentStatement =>
lea -832(%rbp), %rbx # local, i
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
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp loop_start_0
loop_end_1:
#<= ForLoopStatement
#AssignmentStatement =>
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea 40(%rbp), %rbx # param, n
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea 32(%rbp), %rbx # param, m
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $10, %rcx
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
lea -808(%rbp), %rbx # local, lcs_length
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#FunctionCallExpression =>
lea stringLiteral_0(%rip), %rax
push %rax
lea 16(%rbp), %rbx # param, str1
mov (%rbx), %rbx
push %rbx
pop %rbx
push %rbx
lea 24(%rbp), %rbx # param, str2
mov (%rbx), %rbx
push %rbx
pop %rbx
push %rbx
lea -808(%rbp), %rbx # local, lcs_length
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %r9
pop %r8
pop %rdx
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
#AssignmentStatement =>
mov $0, %rax
push %rax
lea -824(%rbp), %rbx # local, lcs_str[]
push %rbx
lea -808(%rbp), %rbx # local, lcs_length
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $1, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %al, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea 32(%rbp), %rbx # param, m
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea 40(%rbp), %rbx # param, n
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -808(%rbp), %rbx # local, lcs_length
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
lea -856(%rbp), %rbx # local, index
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#WhileLoopStatement =>
loop_start_42:
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $0, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jle group_false_label_48
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $0, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jle group_false_label_48
group_true_label_49:
jmp boolean_true_label_46
group_false_label_48:
boolean_false_label_45:
jmp loop_end_43
jmp boolean_no_jmp_label_47
boolean_true_label_46:
boolean_no_jmp_label_47:
lea 16(%rbp), %rbx # param, str1[]
mov (%rbx), %rbx
push %rbx
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $1, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
movzbq (%rbx), %rax
push %rax
lea 24(%rbp), %rbx # param, str2[]
mov (%rbx), %rbx
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $1, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
movzbq (%rbx), %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
jne group_false_label_54
group_true_label_55:
jmp boolean_true_label_52
group_false_label_54:
boolean_false_label_51:
jmp boolean_no_jmp_label_53
boolean_true_label_52:
jmp branch_50
boolean_no_jmp_label_53:
jmp branch_56
jmp branch_compound_if_end_57
branch_50:
#AssignmentStatement =>
lea 16(%rbp), %rbx # param, str1[]
mov (%rbx), %rbx
push %rbx
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $1, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
movzbq (%rbx), %rax
push %rax
lea -824(%rbp), %rbx # local, lcs_str[]
push %rbx
lea -856(%rbp), %rbx # local, index
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $1, %rcx
mul %rcx
push %rax
movq $0, %rax
pop %rcx
add %rcx, %rax
pop %rbx
add %rax, %rbx
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %al, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -856(%rbp), %rbx # local, index
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
lea -856(%rbp), %rbx # local, index
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_57
branch_56:
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $10, %rcx
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
lea -800(%rbp), %rbx # local, dp[]
push %rbx
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
pop %rax
mov $1, %rcx
mul %rcx
mov $8, %rcx
mul %rcx
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov $10, %rcx
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
pop %rbx
pop %rax
cmp %rbx, %rax
jle group_false_label_62
group_true_label_63:
jmp boolean_true_label_60
group_false_label_62:
boolean_false_label_59:
jmp boolean_no_jmp_label_61
boolean_true_label_60:
jmp branch_58
boolean_no_jmp_label_61:
jmp branch_64
jmp branch_compound_if_end_65
branch_58:
#AssignmentStatement =>
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
lea -832(%rbp), %rbx # local, i
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_65
branch_64:
#AssignmentStatement =>
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $1, %rax
push %rax
pop %rbx
pop %rax
sub %rbx, %rax
push %rax
lea -840(%rbp), %rbx # local, j
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
jmp branch_compound_if_end_65
branch_compound_if_end_65:
jmp branch_compound_if_end_57
branch_compound_if_end_57:
updater_44:
jmp loop_start_42
loop_end_43:
#<= WhileLoopStatement
#FunctionCallExpression =>
lea stringLiteral_1(%rip), %rax
push %rax
lea -824(%rbp), %rbx # local, lcs_str
push %rbx
pop %rbx
push %rbx
pop %rdx
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
leave
ret
#<= FunctionDeclare
#FunctionDeclare =>
.global main
main:
push %rbp
mov %rsp, %rbp
add $-48, %rsp
#DeclareStatement =>
#<= DeclareStatement
#FunctionCallExpression =>
lea -16(%rbp), %rbx # local, str1
push %rbx
pop %rbx
push %rbx
mov $10, %rax
push %rax
lea stringLiteral_2(%rip), %rax
push %rax
pop %r8
pop %rdx
pop %rcx
add $-32, %rsp
call strcpy_s
add $32, %rsp
#<= FunctionCallExpression
#DeclareStatement =>
#<= DeclareStatement
#FunctionCallExpression =>
lea -32(%rbp), %rbx # local, str2
push %rbx
pop %rbx
push %rbx
mov $10, %rax
push %rax
lea stringLiteral_3(%rip), %rax
push %rax
pop %r8
pop %rdx
pop %rcx
add $-32, %rsp
call strcpy_s
add $32, %rsp
#<= FunctionCallExpression
#DeclareStatement =>
#FunctionCallExpression =>
lea -16(%rbp), %rbx # local, str1
push %rbx
pop %rbx
push %rbx
pop %rcx
add $-32, %rsp
call strlen
add $32, %rsp
#<= FunctionCallExpression
push %rax
pop %rax
mov %rax, -40(%rbp)
#<= DeclareStatement
#DeclareStatement =>
#FunctionCallExpression =>
lea -32(%rbp), %rbx # local, str2
push %rbx
pop %rbx
push %rbx
pop %rcx
add $-32, %rsp
call strlen
add $32, %rsp
#<= FunctionCallExpression
push %rax
pop %rax
mov %rax, -48(%rbp)
#<= DeclareStatement
#FunctionCallExpression =>
lea -16(%rbp), %rbx # local, str1
push %rbx
pop %rbx
push %rbx
lea -32(%rbp), %rbx # local, str2
push %rbx
pop %rbx
push %rbx
lea -40(%rbp), %rbx # local, m
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -48(%rbp), %rbx # local, n
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %r9
pop %r8
pop %rdx
pop %rcx
add $-32, %rsp
call lcs
add $32, %rsp
#<= FunctionCallExpression
#ReturnStatement =>
mov $0, %rax
push %rax
pop %rax
leave
ret
#<= ReturnStatement
leave
ret
#<= FunctionDeclare

