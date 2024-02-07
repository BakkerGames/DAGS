# DAGS Syntax

DAGS, or Data Access Game Scripts, uses a dictionary of (key,value) pairs. Both the keys and values are strings.

Keys cannot be null, "", or contain only whitespace. Values which are null or "null" or undefined are returned as "". Integer values which would be "" are returned as "0".

Below, "key" specifies the key of a pair in the dictionary, and "value" refers to the value stored for that key.

"Raw value" refers to the value from the dictionary with no processing. "Processed value" will run "value" as a script if it starts with "@" and returns the final result.

Scripts are processed by RunScript(script, result), with "result" a StringBuilder parameter that will return all output.

There is an "InChannel" queue and an "OutChannel" queue which are used for passing string values between DAGS and the calling program. Strings placed on the queues would be handled by the other end as appropriate.


<br>

## Constants

"null"

"true"

"false"


<br>

## Statements

@comment("comment text")

>Used for commenting code. Quotes are recommended.

@exec(value)

>Executes the script specified by "value".

@script(key)

>Runs the script stored in "key".

@set(key,value)

>Sets the value for "key" in the dictionary to "value".

@swap(key1,key2)

>Swaps the values stored in "key1" and "key2".


<br>

## Numeric Statements

@addto(key,value)

>Adds integer "value" to that stored in "key". Stores the answer back in "key".

@subto(key,value)

>Subtracts integer "value" from that stored in "key". Stores the answer back in "key".

@multo(key,value)

>Multiples integer "value" with that stored in "key". Stores the answer back in "key".

@divto(key,value)

>Divides integer stored in "key" by integer "value" and discards the remainder. Stores the answer back in "key".

@modto(key,value)

>Divides integer stored in "key" by integer "value". Stores the remainder back in "key".


<br>

## Output Statements

@msg(key)

>Writes the processed value stored in "key" plus "\n" (two separate characters) to result.

@nl

>Writes "\n" (two separate characters) to result.

@write(value1,value2...)

>Writes all the values concatinated to result.


<br>

## Functions

@abs(value)

>Returns the absolute value of integer "value".

@add(value1,value2)

>Returns the integer answer from adding "value1" and "value2".

@concat(value1,value2...)

>Concatenates all specified values into a single string.

@div(value1,value2)

>Returns the integer answer from dividing "value1" by "value2".

@format(value,v0,v1...)

>Replaces tokens "{0}", "{1}"... in value with v0, v1... and returns the result.

@get(key)

>Returns the raw value for "key" from the dictionary.

@getvalue(key)

>Returns the processed value for "key", running it as a script if necessary.

@lower(value)

>Returns "value" changed to all lowercase.

@mod(value1,value2)

>Returns the integer remainder from dividing "value1" by "value2".

@mul(value1,value2)

>Returns the integer answer from multiplying "value1" and "value2".

@replace(value,old,new)

>Returns "value" with all occurances of "old" changed to "new".

@rnd(value)

>Returns a random integer from 0 to "value" minus 1.

@sub(value1,value2)

>Returns the integer answer from subtracting "value2" from "value1".

@substring(value,start) or @substring(value,start,len)

>Returns the substring starting at position "start". If "len" is specified, return up to "len" characters. Returns "" if out of range.

@trim(value)

>Trims leading and trailing spaces from "value".

@upper(value)

>Returns "value" changed to all uppercase.


<br>

## If Block

@if

>Starts an "@if" block. Followed by condition statements until "@then".

@then

>End of conditions, begin running statements if conditions were true. Required after "@if" and after each "@elseif".

@elseif

>Starts another "@if" block if the first one was not true. Followed by conditions, "@then", and statements. Multiple "@elseif" blocks may be chained one after another. Optional.

@else

>Final statements to be processed if all previous conditions were false. Optional.

@endif

>Ends the "@if" block. Required.


<br>

## If Conditions

Conditions return true/false values, or values that are "truthy" or "falsey".

"true", "t", "on", "yes", "y", "1", and "-1" are truthy.

"false", "f", "off", "no", "n", "0", "null", and "" are falsey.

Conditions are processed strictly left-to-right, with no parenthesis used for grouping. They are connected with "@and" and "@or", which short-circuit when possible. "@not" preceeding the condition will reverse it.

Any functions which returns truthy or falsey values may be defined and used as "@if" conditions.

@eq(value1,value2)

>Checks if the two values are equal. Compares as integers if both convert to integers, otherwise compares as exact strings (ignoring case).

@false(value)

>Returns true if "value" is falsey.

@ge(value1,value2)

>Checks if integer "value1" is greater than or equal to integer "value2". Error if not integers.

@gt(value1,value2)

>Checks if integer "value1" is greater than integer "value2". Error if not integers.

@isnull(value)

>Returns true if "value" is "" or "null",

@isscript(value)

>Checks if "value" starts with "@".

@le(value1,value2)

>Checks if integer "value1" is less than or equal to integer "value2". Error if not integers.

@lt(value1,value2)

>Checks if integer "value1" is less than integer "value2". Error if not integers.

@ne(value1,value2)

>Checks if the two values are not equal. Compares as integers if both convert to integers, otherwise compares as exact strings (ignoring case).

@rand(value)

>Checks if a random integer 0-99 is less than integer "value" 1-100. Shortened version of "@lt(@rnd(100),value)".

@true(value)

>Returns true if "value" is truthy.


<br>

## Condition Connectors/Modifiers

@and

>Stops processing if the condition so far is false and jumps over the "@then" statements to "@elseif", "@else", or "@endif". Otherwise continues.

@or

>Stops processing if the condition so far is true and jumps to the "@then" statements. Otherwise continues.

@not

>Reverses the answer on the next condition.


<br>

## For Loop

@for(token,start,end)

>Executes the code in the "for" block multiple times, by replacing "$token" (the token with a leading "$") anywhere in it with the numeric values from "start" to "end" (inclusive). Tokens can be anything, such as "i", "x", "y", "token". Nesting is allow if the tokens are different.

@endfor

>Marks the end of the "@for" loop.


<br>

## ForEachKey Loop

@foreachkey(token,prefix) or @foreachkey(token,prefix,suffix)

>Executes the code in the "foreachkey" block multiple times, by replacing "$token" (the token with a leading "$") anywhere in it. It loops through all the keys in the dictionary which start with "prefix" and optionally end with "suffix". The value replaced for "$token" is the remaining part of the key after the prefix and the optional suffix are removed. Tokens can be anything, but should have no spaces or special characters. Nesting is allow if the tokens are different. Prefix and suffix are case sensitive. Note that the order of keys returned is not deterministic.

@endforeachkey

>Marks the end of the "@foreachkey" loop.


<br>

## ForEachList Loop

@foreachlist(token,name)

>Executes the code in the "foreachlist" block multiple times, by replacing "$token" (the token with a leading "$") anywhere in it with all the comma-separated values in the list "name". The script in the block will be executed once per value. Tokens can be anything, but should have no spaces or special characters. Nesting is allow if the tokens are different.

>Note: Any values within the list which are "" or "null" will be ignored.

@endforeachlist

>Marks the end of the "@foreachlist" loop.


<br>

## List Statements/Functions

These commands allow named lists of values to be stored as a single group instead of separate key/value pairs. They can be indexed by number, appended to, and cleared. The items will be separated by commas, so commas in the values will be replaced by the token "\x2C" when stored. Empty strings "" will be replaced by "null" when stored so they can be handled properly.

@addlist(name,value)

>Adds value to the end of the list "name".

@clearlist(name)

>Clears the list "name".

@getlist(name,pos)

>Gets the value at position "pos" (starting at 0) for the list "name". If "pos" is beyond the end of the list, "" is returned.

@insertatlist(name,pos,value)

>Inserts "value" at position "pos" (starting at 0) from list, shifting all later ones. If "pos" is past the end of the list, all values from the end up to "pos" are filled with "null" and then value is added.

@listlength(name)

>Returns the number of items in list "name".

@removeatlist(name,pos)

>Removes the value at position "pos" (starting at 0) from list, shifting all later ones. If "pos" is past the end of the list, nothing happens.

@setlist(name,pos,value)

>Sets the value at position "pos" (starting at 0) for the list "name". If "pos" is beyond the end of the list, all values from the end up to "pos" are filled with "null" and then value is added.


<br>

## Array Statements/Functions

These commands allow a two-dimensional array of values to be stored as a group. They are sparse arrays with unspecified values returned as "". The items will be separated by commas, so commas in the values will be replaced by the token "\x2C" when stored. Empty strings "" will be replaced by "null" when stored so they can be handled properly. Arrays are stored in the dictionary with the keys "{name}.{y}", with "{y}" as the row number.

Note that the array values are referenced by row (y) first and then column (x), both starting at 0. Negative indexes throw an error.

@cleararray(name)

>Clears all values from the array "name".

@getarray(name,y,x)

>Gets the value at position "y,x" (starting at 0,0) for the array "name". If either "y" or "x" is beyond the edge of the stored values, "" is returned.

@setarray(name,y,x,value)

>Sets the value at position "y,x" (starting at 0,0) for the array "name". If either "y" or "x" is beyond the edge of the stored values, missing values will be set to "null" as needed before adding "value".


<br>

## In/Out Channel Commands

@setoutchannel(value)

>Adds the value to the OutChannel queue. The calling program would be looking for these values and know how to process them.

@getinchannel

>Gets the next value from the InChannel queue and returns it. The script would use or process that value as appropriate.


<br>

## Public Interface

new Dags(IDictionary<string, string> dictionary)

>Create a new DAGS object connected to an existing dictionary.

RunScript(string script, StringBuilder result)

>Runs the specified script and return any answers in result.

Queue<string> InChannel

>Queue for recieving information from the calling program.

Queue<string> OutChannel

>Queue for sending information back to the calling program.

bool ValidateDictionary(StringBuilder result)

>Checks that the dictionary is valid for use with DAGS. Returns all errors found. Includes checking all scripts with ValidateScript.

bool ValidateScript(string script, StringBuilder result)

>Checks that the script has correct syntax. Returns all errors found.

string PrettyScript(string script)

>Returns the script with line splitting and indenting for more readable code. Useful for exporting or for an editor program.

string Help()

>Returns a syntax listing of all DAGS commands.


<br>

## Debugging

Debugging is available in a limited form. It is for stepping through a script and figuring out issues. It steps through each statement and returns each result separately. It will step through each condition in an "@if" block but only at the top level, not for nested "@if"s. It does perform any changes on the data. The calling program starts with DebugScript() and then calls DebugNext() until done.

DebugScript(string script, StringBuilder result)

>Starts debugging the specified "script". If "script" doesn't start with "@" then it is used as a key to get the script value. Performs the first step and returns the result.

DebugNext(StringBuilder result)

>Performs the next step in "script" and returns the results. Returns "Debug done." if at the end.

bool DebugDone()

>Returns True when "script" has finished.


<br>

## Adding new functions to DAGS

Functions in DAGS are either built-in, such as those above, or are entries in the dictionary having keys starting with "@". You can add as many new functions to DAGS as you need. They are very useful when functionality is needed in several places, instead of repeating the same code each time.

Function names must be unique. They can't duplicate any of the built-in function names.

A function with no parameters would have a "key" of the desired function name: "@myfunc". The "value" would contain a script to be run whenever that function was called. The "value" can also be a simple text value to be returned whenever the function is called.

A function with parameters would have a "key" with the specified name and would list the parameters in parentheses separated by commas: "@myfunc(x,y,z)". The "value" would be a script with replaceable parameter values such as "$x", "$y", "$z" somewhere in it. Any number of parameters may be specified, but at least one. The parameter names can be anything desired but should have no spaces or special characters, or conflict with other parameters from @for(), @foreachkey(), or @foreachlist() statements within the script.

Examples:

@quitmsg=Are you sure you want to quit?

@score=@write("You have a score of ",@get(value.score)," out of ",@get(value.maxscore)," points.") @nl @nl

@moveto(x,y)=@comment("moves the item to a location") @set(item.$x.location,$y)

@unknown(x)=@write("I don't understand \"$x\".\n")