
#=================================================#

#Main() -> GenerateAsm() -> EmitAsm()
.data

stringLiteral_0:  .asciz "%d -> "
stringLiteral_1:  .asciz "NULL\n"
stringLiteral_2:  .asciz "Original list: "
stringLiteral_3:  .asciz "Reversed list: "

.text

#FunctionDeclare =>
.global add_node
add_node:
push %rbp
mov %rsp, %rbp
add $-16, %rsp
lea 16(%rbp), %rax
mov %rcx, (%rax)
lea 24(%rbp), %rax
mov %rdx, (%rax)
#DeclareStatement =>
#FunctionCallExpression =>
mov $20, %rax
push %rax
pop %rcx
add $-32, %rsp
call malloc
add $32, %rsp
#<= FunctionCallExpression
push %rax
pop %rax
mov %rax, -8(%rbp)
#<= DeclareStatement
#AssignmentStatement =>
lea 24(%rbp), %rbx # param, value
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -8(%rbp), %rbx # local, new_node->value
push %rbx
pop %rbx
mov (%rbx), %rbx
add $0, %rbx
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea 16(%rbp), %rbx # param, head
mov %rbx, %rbx
push %rbx
pop %rbx
mov (%rbx), %rbx
mov (%rbx), %rbx
push %rbx
lea -8(%rbp), %rbx # local, new_node->next
push %rbx
pop %rbx
mov (%rbx), %rbx
add $8, %rbx
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -8(%rbp), %rbx # local, new_node
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea 16(%rbp), %rbx # param, head
mov %rbx, %rbx
push %rbx
pop %rbx
mov (%rbx), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
leave
ret
#<= FunctionDeclare
#FunctionDeclare =>
.global print_list
print_list:
push %rbp
mov %rsp, %rbp
add $-16, %rsp
lea 16(%rbp), %rax
mov %rcx, (%rax)
#DeclareStatement =>
lea 16(%rbp), %rbx # param, head
mov %rbx, %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov %rax, -8(%rbp)
#<= DeclareStatement
#WhileLoopStatement =>
loop_start_0:
lea -8(%rbp), %rbx # local, current
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $0, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
je group_false_label_6
group_true_label_7:
jmp boolean_true_label_4
group_false_label_6:
boolean_false_label_3:
jmp loop_end_1
jmp boolean_no_jmp_label_5
boolean_true_label_4:
boolean_no_jmp_label_5:
#FunctionCallExpression =>
lea stringLiteral_0(%rip), %rax
push %rax
lea -8(%rbp), %rbx # local, current->value
push %rbx
pop %rbx
mov (%rbx), %rbx
add $0, %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rdx
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
#AssignmentStatement =>
lea -8(%rbp), %rbx # local, current->next
push %rbx
pop %rbx
mov (%rbx), %rbx
add $8, %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -8(%rbp), %rbx # local, current
push %rbx
pop %rbx
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
lea stringLiteral_1(%rip), %rax
push %rax
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
leave
ret
#<= FunctionDeclare
#FunctionDeclare =>
.global reverse_list
reverse_list:
push %rbp
mov %rsp, %rbp
add $-32, %rsp
lea 16(%rbp), %rax
mov %rcx, (%rax)
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
mov %rax, -8(%rbp)
#<= DeclareStatement
#DeclareStatement =>
lea 16(%rbp), %rbx # param, head
mov %rbx, %rbx
push %rbx
pop %rbx
mov (%rbx), %rbx
mov (%rbx), %rbx
push %rbx
pop %rax
mov %rax, -16(%rbp)
#<= DeclareStatement
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
mov %rax, -24(%rbp)
#<= DeclareStatement
#WhileLoopStatement =>
loop_start_8:
lea -16(%rbp), %rbx # local, current
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $0, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
je group_false_label_14
group_true_label_15:
jmp boolean_true_label_12
group_false_label_14:
boolean_false_label_11:
jmp loop_end_9
jmp boolean_no_jmp_label_13
boolean_true_label_12:
boolean_no_jmp_label_13:
#AssignmentStatement =>
lea -16(%rbp), %rbx # local, current->next
push %rbx
pop %rbx
mov (%rbx), %rbx
add $8, %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -24(%rbp), %rbx # local, next
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -8(%rbp), %rbx # local, prev
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -16(%rbp), %rbx # local, current->next
push %rbx
pop %rbx
mov (%rbx), %rbx
add $8, %rbx
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -16(%rbp), %rbx # local, current
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -8(%rbp), %rbx # local, prev
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
#AssignmentStatement =>
lea -24(%rbp), %rbx # local, next
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -16(%rbp), %rbx # local, current
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
updater_10:
jmp loop_start_8
loop_end_9:
#<= WhileLoopStatement
#AssignmentStatement =>
lea -8(%rbp), %rbx # local, prev
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea 16(%rbp), %rbx # param, head
mov %rbx, %rbx
push %rbx
pop %rbx
mov (%rbx), %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
leave
ret
#<= FunctionDeclare
#FunctionDeclare =>
.global free_list
free_list:
push %rbp
mov %rsp, %rbp
add $-16, %rsp
lea 16(%rbp), %rax
mov %rcx, (%rax)
#DeclareStatement =>
lea 16(%rbp), %rbx # param, head
mov %rbx, %rbx
push %rbx
pop %rbx
mov (%rbx), %rbx
mov (%rbx), %rbx
push %rbx
pop %rax
mov %rax, -8(%rbp)
#<= DeclareStatement
#WhileLoopStatement =>
loop_start_16:
lea -8(%rbp), %rbx # local, current
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
mov $0, %rax
push %rax
pop %rbx
pop %rax
cmp %rbx, %rax
je group_false_label_22
group_true_label_23:
jmp boolean_true_label_20
group_false_label_22:
boolean_false_label_19:
jmp loop_end_17
jmp boolean_no_jmp_label_21
boolean_true_label_20:
boolean_no_jmp_label_21:
#DeclareStatement =>
lea -8(%rbp), %rbx # local, current
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rax
mov %rax, 0(%rbp)
#<= DeclareStatement
#AssignmentStatement =>
lea -8(%rbp), %rbx # local, current->next
push %rbx
pop %rbx
mov (%rbx), %rbx
add $8, %rbx
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
lea -8(%rbp), %rbx # local, current
push %rbx
pop %rbx
push %rbx
pop %rbx
pop %rax
mov %rax, (%rbx)
#<= AssignmentStatement
updater_18:
jmp loop_start_16
loop_end_17:
#<= WhileLoopStatement
leave
ret
#<= FunctionDeclare
#FunctionDeclare =>
.global main
main:
push %rbp
mov %rsp, %rbp
add $-8, %rsp
#DeclareStatement =>
mov $0, %rax
push %rax
pop %rax
mov %rax, -8(%rbp)
#<= DeclareStatement
#FunctionCallExpression =>
lea -8(%rbp), %rbx # local, &head
push %rbx
pop %rbx
push %rbx
mov $10, %rax
push %rax
pop %rdx
pop %rcx
add $-32, %rsp
call add_node
add $32, %rsp
#<= FunctionCallExpression
#FunctionCallExpression =>
lea -8(%rbp), %rbx # local, &head
push %rbx
pop %rbx
push %rbx
mov $20, %rax
push %rax
pop %rdx
pop %rcx
add $-32, %rsp
call add_node
add $32, %rsp
#<= FunctionCallExpression
#FunctionCallExpression =>
lea -8(%rbp), %rbx # local, &head
push %rbx
pop %rbx
push %rbx
mov $30, %rax
push %rax
pop %rdx
pop %rcx
add $-32, %rsp
call add_node
add $32, %rsp
#<= FunctionCallExpression
#FunctionCallExpression =>
lea -8(%rbp), %rbx # local, &head
push %rbx
pop %rbx
push %rbx
mov $40, %rax
push %rax
pop %rdx
pop %rcx
add $-32, %rsp
call add_node
add $32, %rsp
#<= FunctionCallExpression
#FunctionCallExpression =>
lea -8(%rbp), %rbx # local, &head
push %rbx
pop %rbx
push %rbx
mov $50, %rax
push %rax
pop %rdx
pop %rcx
add $-32, %rsp
call add_node
add $32, %rsp
#<= FunctionCallExpression
#FunctionCallExpression =>
lea stringLiteral_2(%rip), %rax
push %rax
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
#FunctionCallExpression =>
lea -8(%rbp), %rbx # local, head
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rcx
add $-32, %rsp
call print_list
add $32, %rsp
#<= FunctionCallExpression
#FunctionCallExpression =>
lea -8(%rbp), %rbx # local, &head
push %rbx
pop %rbx
push %rbx
pop %rcx
add $-32, %rsp
call reverse_list
add $32, %rsp
#<= FunctionCallExpression
#FunctionCallExpression =>
lea stringLiteral_3(%rip), %rax
push %rax
pop %rcx
add $-32, %rsp
call printf
add $32, %rsp
#<= FunctionCallExpression
#FunctionCallExpression =>
lea -8(%rbp), %rbx # local, head
push %rbx
pop %rbx
mov (%rbx), %rax
push %rax
pop %rcx
add $-32, %rsp
call print_list
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

