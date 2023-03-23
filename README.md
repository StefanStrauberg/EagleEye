# EagleEye
It's my first very simple project where I try to develop SIEM (Scurity Information and Event Management) aka ELK stack.

Service application FGLogDog that receives fortigate logs and converts them to bson format.
FGLogDog based on the DDD approach and separate into several layers.
Application layer:
	The main business logic of the application.
Receiver layers:
	The UDP layer allow for the application to receive messages over the UDP protocol.
	The TCP layer allow for the application to receive messages over the TCP protocol. (Temporary not working)
Producer layers:
	RabbitMQ layer allow for the application to produce messages over the AMQP protocol to the RabbitMQ message broker.
	Before the data is sent to next application, this layer accesses the ParserFactory layer,
	which converts them to bson format.
TemporaryBuffer layer:
	The main buffer for temporary storage incoming data in the bytecode format.

WebAPI application EagleEye that receive logs via bson format and storage them into Mongo Database.
EagleEye based on the DDD approach and separate into several layers.
Application layer:
	The main business logic of the application.
Infrastructure layer:
	The main layer for communication with the Database.
TemporaryBuffer layer:
	The main buffer for temporary storage incoming data in the bytecode format.
	Before EagleEye clears the buffer and save data to databse, data is stored in the buffer for a while.
	Buffer has parameter:
	SizeOfBuffer parameter - represents the size of buffer when the buffer completely full,
	the storage data is automatically saved to the databse.
TemporaryBuffer service layer:
	This service layer represent background processes of application.
	Timer process has parameter:
	DelayTimer parameter - represents the time in seconds when the buffer will be cleared if it is doesn't completely full.
Receiver layer:
	The RabbitMQMessenger layer allow for the application to receive messages over the AMQP protocol from message broker.
