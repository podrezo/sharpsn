SharpSN
===============

SharpSN is a .NET library that lets you generate and verify serial numbers offline. It is not really meant to be used as is but to be extended a bit and then integrated into your own code base. The reason for this is not that it doesn't work as is, but because you would probably want some sort of creative variation on the algorithm to be at least slightly more secure than an algorithm that is freely available for all to see.

Quick Start
===============

Add a reference to the library and import it

`using SharpSN;`

All serial numbers will be expected to consist of characters A-F0-9 (hex characters, uppercase only) in identically sized sections of *N* characters with *M* total sections (where *M*>2). The reason why *M*>2 is because at least one section will be purely random characters, and at least one section will be related to the hash (your choice of algorithm) of the first *M*-1 sections. Initialize a new generator/verifier instance by telling it these parameters like so:

`var generator = new SerialNumbers(numberOfSections, charsPerSection, SHA256.Create());`

In this example `SHA256` is part of `System.Security.Cryptography`.

To generate a hash simply call `generator.GenerateNewSerialNumber()` which will return you a string like so: `23ADD2-88A748-D41235-ECEB24-503208` - this is your random serial number.

To verify that it is a valid one, simply call `generator.VerifySerialNumber("23ADD2-88A748-D41235-ECEB24-503208")` which will return `true`. Changing even one digit will make the result false since the the last section is the hash of the entire first part of the key.


SharpSN is licensed under the MIT license.

License
===============

SharpSN is licensed under the MIT license.

Copyright (c) 2015 Petro Podrezo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.